import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map } from 'rxjs/operators';
import { BehaviorSubject, Observable } from 'rxjs';
import { ApplicationUser } from '../models/account/application-user.model';
import { ApplicationUserLogin } from '../models/account/application-user-login.model';
import { environment } from 'src/environments/environment';
import { ApplicationUserCreate } from '../models/account/application-user-create.model';

@Injectable({
  providedIn: 'root'
})
export class AccountService {
  
  private currentUserSubject$: BehaviorSubject<ApplicationUser> 

  constructor(
    private http: HttpClient
  ) { 
    this.currentUserSubject$ = new BehaviorSubject<ApplicationUser>(JSON.parse(localStorage.getItem('blogLab-currentUser') || '{}'));
  }

  // Object not assignable? See if any type works 
  login(model: ApplicationUserLogin) : Observable<ApplicationUser> {
    return this.http.post(`${environment.webApi}/Account/login`, model).pipe(
      map((user : any) => { 
        
        if (user) {
          localStorage.setItem('blogLab-currentUser', JSON.stringify(user));
          this.setCurrentUser(user);
        }

        return user; 
      })
    );
  }

  setCurrentUser(user: ApplicationUser) {
    this.currentUserSubject$.next(user);
  }

  register(model: ApplicationUserCreate) : Observable<ApplicationUser> {
    return this.http.post(`${environment.webApi}/Account/register`, model).pipe(
      map((user : any) => { 
        
        if (user) {
          localStorage.setItem('blogLab-currentUser', JSON.stringify(user));
          this.setCurrentUser(user);
        }

        return user; 
      })
    );
  }

  public get currentUserValue(): ApplicationUser {
    return this.currentUserSubject$.value;
  }

  logout() {
    localStorage.removeItem('blogLab-currentUser');
    // maybe turn off strictNullChecks in tsconfig
    this.currentUserSubject$.next(null as any);
  }
}
