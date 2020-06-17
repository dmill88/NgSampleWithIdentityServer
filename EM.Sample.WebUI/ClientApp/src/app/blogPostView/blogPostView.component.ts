import { Component, OnInit, OnDestroy } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Observable, Subscription } from 'rxjs';
import { BlogQueriesService } from './../blogService/blogQueries.service';
import { Blog, Post, BlogPost } from './../models/blog.models';
import { ControlValidationService } from '../shared/controlValidation.service';


@Component({
  templateUrl: './blogPostView.component.html',
  providers: [BlogQueriesService]
})
export class BlogPostViewComponent implements OnInit, OnDestroy {
  public blogPost: BlogPost = null;
  public title: string = 'Blog Post';
  public isLoaded: boolean = false;
  public errors: string[];

  constructor(
    private _route: ActivatedRoute,
    private _blogQueriesService: BlogQueriesService
  ) {
  }

  public ngOnInit(): void {
    console.log('BlogPostViewComponent.ngOnInit()');

    this._route.params.subscribe(params => {
      const id = +params['id'];
      if (id !== null) {
        this.loadPostBlog(id);
      }
    }, error => {
        console.log(JSON.stringify(error));
        this.errors = ControlValidationService.parseWebApiErrors(error);
    });
  }

  ngOnDestroy() {
    console.log('BlogPostViewComponent.ngOnDestroy');
  }

  public loadPostBlog(id: number): void {
    this.isLoaded = false;
    this._blogQueriesService.getBlogPost(id).subscribe(blogPost => {
      this.blogPost = blogPost;
      this.title = blogPost.title;
    }, error => {
      this.isLoaded = true;
      console.log(JSON.stringify(error));
      this.errors = ControlValidationService.parseWebApiErrors(error);
    }, () => {
      this.isLoaded = true;
    });
  }

}
