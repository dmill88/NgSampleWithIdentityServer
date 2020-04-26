import { extend } from "webdriver-js-extender";

export enum BlogStatuses {
  Draft = 1,
  Review = 2,
  Published = 3,
  Archived = 4
}

export enum PostStatuses {
  Draft = 1,
  Review = 2,
  Published = 3,
  Archived = 4
}

export enum CommentStatuses {
  MemberOnly = 1,
  Locked = 2,
  Disabled = 3,
  Anonymous = 4
}

export class Author {
  constructor() {
  }
  public id: number;
  public userId: string;
  public firstName: string;
  public lastName: string;
  public alias: string;
  public bio: string;
  public portraitImgId: number;
  public createdAt: Date;
  public updatedAt: Date;
  public active: boolean;
  public displayName: string;
}

export class BlogListItem {
  constructor() {
  }
  public id: number = 0;
  public guid: string = '';
  public name: string;
  public displayName: string;
}

export class Blog {
  constructor() {
  }
  public id: number = 0;
  public guid: string = '';
  public name: string;
  public description: string;
  public displayName: string;
  public displayOrder: number = 1;
  public blogStatusId: number = <number>BlogStatuses.Draft;
  public blogStatus: BlogStatuses = BlogStatuses.Draft;
  public primaryAuthorId: number;
  public primaryAuthor: Author;
}

export class Post {
  constructor() {
    this.tags = new Array<string>();
  }

  public id: number;
  public guid: string;
  public title: string;
  public postContent: string;
  public excerpt: string;
  public primaryAuthorId: number;
  public primaryAuthor: Author;
  public postStatusId: PostStatuses = PostStatuses.Draft;
  public commentStatusId: CommentStatuses = CommentStatuses.MemberOnly;
  public commentCount: number = 0;
  public createdAt: Date;
  public updatedAt: Date;
  public tags: string[];
}

export class PostListItem {
  constructor() {
  }

  public id: number;
  public guid: string;
  public title: string;
  public postContent: string;
  public excerpt: string;
  public primaryAuthorId: number;
  public primaryAuthor: Author;
  public postStatusId: PostStatuses = PostStatuses.Draft;
  public commentStatusId: CommentStatuses = CommentStatuses.MemberOnly;
  public commentCount: number = 0;
  public createdAt: Date;
  public updatedAt: Date;
}

export class BlogPost extends Post {
  public blogId: number;
  public blogDisplayName: string;
}



