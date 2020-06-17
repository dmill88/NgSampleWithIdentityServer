import { Component, OnInit, OnDestroy } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Observable, Subscription } from 'rxjs';
import { BlogQueriesService } from './../blogService/blogQueries.service';
import { Blog, BlogPost, Post } from './../models/blog.models';
import { ControlValidationService } from '../shared/controlValidation.service';


@Component({
  templateUrl: './blogView.component.html',
  providers: [BlogQueriesService]
})
export class BlogViewComponent implements OnInit, OnDestroy {
  public blog: Blog = null;
  public title: string = 'Blog';
  public isLoaded: boolean = false;
  public posts: Array<Post>;
  public errors: string[];

  constructor(
    private _route: ActivatedRoute,
    private _blogQueriesService: BlogQueriesService
  ) {
  }

  public ngOnInit(): void {
    console.log('BlogViewComponent.ngOnInit()');

    this._route.params.subscribe(params => {
      const id = +params['id'];
      if (id != null) {
        this.loadBlog(id);
      }
    }, error => {
        console.log(JSON.stringify(error));
        this.errors = ControlValidationService.parseWebApiErrors(error);
    });
  }

  ngOnDestroy() {
    console.log('BlogViewComponent.ngOnDestroy');
  }

  public loadBlog(id: number): void {
    this.isLoaded = false;
    this._blogQueriesService.getBlog(id).subscribe(blog => {
      this.blog = blog;
      this.title = blog.displayName;

      this._blogQueriesService.getBlogPosts(id).subscribe(pagedResult => {
        this.posts = pagedResult.data as Post[];
      }, error => {
          this.errors = ControlValidationService.parseWebApiErrors(error);
      });

    }, error => {
      this.isLoaded = true;
      this.errors = ControlValidationService.parseWebApiErrors(error);
      console.log(JSON.stringify(error));
    }, () => {
      this.isLoaded = true;
    });
  }

}
