import { Component, OnInit, OnDestroy, Input } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { SelectItem, SelectItemGroup } from 'primeng/api';
import { BlogQueriesService } from './../blogService/blogQueries.service';
import { Blog, BlogListItem, BlogStatuses, PostStatuses, Post } from './../models/blog.models';
import { ControlValidationService } from './../shared/controlValidation.service';

@Component({
  selector: 'blog-post-list',
  templateUrl: './blogPostList.component.html',
  providers: [BlogQueriesService]
})
export class BlogPostListComponent implements OnInit, OnDestroy {
  @Input() public blogId: number = 0;
  public blogPosts: Array<Post> = null;
  public errors: Array<string> = new Array<string>();
  public loading: boolean = false;
  public postStatusId: number = PostStatuses.Published as number;
  public postStatuses: SelectItem[];
  //public xxblogId: number;

  constructor(
    private _blogQueries: BlogQueriesService,
    private _route: ActivatedRoute,
    private _router: Router
  ) {
  }
  public ngOnInit(): void {
    console.log('BlogPostListComponent.ngOnInit called.');
    this.postStatuses = this._blogQueries.getPostStatuses();
    this.getBlogPosts(this.blogId);
  }

  ngOnDestroy() {
    console.log('BlogListComponent.ngOnDestroy');
  }

  public getBlogPosts(blogId: number): void {
    this.loading = true;
    // _blogQueriesService.getAllBlogPosts
    this._blogQueries.getBlogPosts(blogId).subscribe(pagedResult => {
      this.loading = false;
      this.blogPosts = pagedResult.data as Post[];
    }, error => {
      this.loading = false;
      debugger;
      this.errors = ControlValidationService.parseWebApiErrors(error);
    });
  }

  public routeToAddBlogPost() {
    this._router.navigate(['addBlogPost', this.blogId]);
  }

  public routeToEditBlogPost(id: number) {
    this._router.navigate(['editBlogPost', id]);
  }

}
