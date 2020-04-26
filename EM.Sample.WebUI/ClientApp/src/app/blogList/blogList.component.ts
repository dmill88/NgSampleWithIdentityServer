import { Component, OnInit, OnDestroy } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { SelectItem, SelectItemGroup } from 'primeng/api';
import { BlogQueriesService } from './../blogService/blogQueries.service';
import { Blog, BlogListItem, BlogStatuses } from './../models/blog.models';
import { ControlValidationService } from './../shared/controlValidation.service';

@Component({
  selector: 'blog-list',
  templateUrl: './blogList.component.html',
  providers: [BlogQueriesService]
})
export class BlogListComponent implements OnInit, OnDestroy {
  public blogs: Array<BlogListItem> = null;
  public errors: Array<string> = null;
  public loading: boolean = false;
  public blogStatusId: number = <number>BlogStatuses.Published;
  public blogStatuses: SelectItem[];

  constructor(
    private _blogQueries: BlogQueriesService,
    private _route: ActivatedRoute,
    private _router: Router
  ) {
  }
  public ngOnInit(): void {
    console.log('BlogListComponent.ngOnInit called.');

    this.blogStatuses = this._blogQueries.getBlogStatusSelectList();

    this._route.queryParamMap.subscribe(paramMap => {
      let primaryAuthorId: number = null;
      let val: string = paramMap.get('statusId');
      if (+val) {
        this.blogStatusId = +val;
      }
      val = paramMap.get('primaryAuthorId');
      if (+val) {
        primaryAuthorId = +val;
      }
      this.getBlogs(this.blogStatusId, primaryAuthorId);
    });
  }

  ngOnDestroy() {
    console.log('BlogListComponent.ngOnDestroy');
  }

  public handleBlogStatusChange(event: any) {
    this.getBlogs(event.value, null);
  }

  public getBlogs(statusId: BlogStatuses = BlogStatuses.Draft, primaryAuthorId: number = null): void {
    this.loading = true;
    this._blogQueries.getBlogs(statusId, primaryAuthorId).subscribe(blogs => {
      this.loading = false;
      this.blogs = blogs;
    }, error => {
      this.loading = false;
      //debugger;
      this.errors = ControlValidationService.parseWebApiErrors(error);
    });
  }

  public routeToAddBlog() {
    this._router.navigate(['blogEditor', 0]);
  }

  public routeToEditBlog(blogId: number) {
    this._router.navigate(['blogEditor', blogId]);
  }


}
