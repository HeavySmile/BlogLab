export class BlogComment {
    constructor(
        public blogCommentId: number, 
        public blogId: number, 
        public content: string,
        public username: string,
        public applicationUserId: number,
        public publishData: Date,
        public updateDate: Date,
        public parentBlogCommentId?: number
    ) {
        
        
    }
}