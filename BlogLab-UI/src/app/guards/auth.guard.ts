import { inject } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivateFn, Router, RouterStateSnapshot } from '@angular/router';
import { AccountService } from '../services/account.service';
//
// !!!!!! Check for validity
//
class PermissionsService {

  constructor(
    private accountService: AccountService,
    private router: Router
  ) {}

  canActivate(next: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean {
    const currentUser = this.accountService.currentUserValue;
    const isLoggedIn = currentUser && currentUser.token;

    if (isLoggedIn) return true;

    this.router.navigate(['/']);

    return false;
  }
}

export const AuthGuard: CanActivateFn = (route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean => {
  return inject(PermissionsService).canActivate(route, state);
};
