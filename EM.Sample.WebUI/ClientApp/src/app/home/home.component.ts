import { Component, OnInit, OnDestroy } from '@angular/core';
import { Blog, BlogListItem, BlogStatuses } from './../models/blog.models';
import { ControlValidationService } from '../shared/controlValidation.service';
import { BlogQueriesService } from './../blogService/blogQueries.service';
import { AuthService } from '../shared/auth-service.component';
import { Constants } from '../constants';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  providers: [BlogQueriesService]
})
export class HomeComponent implements OnInit, OnDestroy {
  public publishedBlogs: Array<BlogListItem> = null;
  public errors: Array<string> = new Array<string>();
  public isLoggedIn: boolean = false;
  public isBlogEditor: boolean = false;

  constructor(
    private _blogQueries: BlogQueriesService,
    private _authService: AuthService
  ) {
  }
  public ngOnInit(): void {
    this._authService.loginChanged.subscribe(isLoggedIn => {
        //debugger;
        console.log('HomeComponent loginChanged', isLoggedIn);
        this.isLoggedIn = isLoggedIn;
        if (this._authService.authContext) {
          this.isBlogEditor = this._authService.authContext.isBlogEditor;
        }
    });
    console.log('HomeComponent.ngOnInit called.');
    this.updateLoginStatus();
    this.getBlogs();
  }

  public getBlogs(statusId: BlogStatuses = BlogStatuses.Published): void {
    this._blogQueries.getBlogs(statusId).subscribe(blogs => {
      this.publishedBlogs = blogs;
    }, (error) => {
      //debugger;
      this.errors = ControlValidationService.parseWebApiErrors(error);
    });
  }

  private updateLoginStatus(): void {
    this._authService.isLoggedIn().then(loggedIn => {
      this.isLoggedIn = loggedIn;
      if (this._authService.authContext != null) {
        this.isBlogEditor = this._authService.authContext.isInRole(Constants.userRoles.blogEditor);;
      }
    })
  }

  ngOnDestroy() {
    console.log('HomeComponent.ngOnDestroy');
  }

}
