import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { RouterModule } from '@angular/router';

import { AccordionModule } from 'primeng/accordion';     //accordion and accordion tab
import { ListboxModule } from 'primeng/listbox';
import { DropdownModule } from 'primeng/dropdown';
import { ButtonModule } from 'primeng/button';
import { InputTextModule } from 'primeng/inputtext';
import { SpinnerModule } from 'primeng/spinner';
import { PickListModule } from 'primeng/picklist';
import { InputTextareaModule } from 'primeng/inputtextarea';
import { RatingModule } from 'primeng/rating';
import { EditorModule } from 'primeng/editor';
import { TableModule } from 'primeng/table';
import { ToolbarModule } from 'primeng/toolbar';

import { SharedModule } from './shared/shared.module';
import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { HomeComponent } from './home/home.component';
import { FetchDataComponent } from './fetch-data/fetch-data.component';
import { BlogEditorComponent } from './blogEditor/blogEditor.component';
import { BlogListComponent } from './blogList/blogList.component'
import { BlogPostListComponent } from './blogPostList/blogPostList.component';
import { BlogPostEditorComponent } from './blogPost/blogPostEditor.component';
import { ControlValidationMessageComponent } from './shared/controlValidationMessage.component';
import { ErrorListComponent } from './errorList/errorList.component';
import { TagEditorComponent } from './tagsEditor/tagsEditor.component';
import { UnauthorizedComponent } from './home/unauthorized.component';
import { SigninRedirectCallbackComponent } from './home/signin-redirect-callback.component';
import { SignoutRedirectCallbackComponent } from './home/signout-redirect-callback.component';
import { BlogViewComponent } from './blogView/blogView.component';
import { BlogPostViewComponent } from './blogPostView/blogPostView.component';
import { RoleRouteGuard } from './shared/role-route-gaurd';
import { Constants } from './constants';

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    FetchDataComponent,
    BlogEditorComponent,
    BlogViewComponent,
    BlogPostViewComponent,
    BlogPostEditorComponent,
    ControlValidationMessageComponent,
    ErrorListComponent,
    TagEditorComponent,
    BlogListComponent,
    BlogPostListComponent,
    UnauthorizedComponent
  ],
  imports: [
    //BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    BrowserModule,
    BrowserAnimationsModule,
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,

    AccordionModule,
    ListboxModule,
    DropdownModule,
    ButtonModule,
    InputTextModule,
    SpinnerModule,
    PickListModule,
    InputTextareaModule,
    EditorModule,
    TableModule,
    ToolbarModule,
    RatingModule,

    SharedModule,

    RouterModule.forRoot([
      { path: 'blogView/:id', component: BlogViewComponent },
      { path: 'blogPostView/:id', component: BlogPostViewComponent },
      { path: 'blogList', component: BlogListComponent, canActivate: [RoleRouteGuard], data: { role: Constants.userRoles.blogEditor } },
      { path: 'blogEditor/:id', component: BlogEditorComponent, canActivate: [RoleRouteGuard], data: { role: Constants.userRoles.blogEditor } },
      { path: 'addBlogPost/:blogId', component: BlogPostEditorComponent, canActivate: [RoleRouteGuard], data: { role: Constants.userRoles.blogEditor } },
      { path: 'editBlogPost/:id', component: BlogPostEditorComponent, canActivate: [RoleRouteGuard], data: { role: Constants.userRoles.blogEditor } },
      { path: 'fetch-data', component: FetchDataComponent },
      { path: 'signin-callback', component: SigninRedirectCallbackComponent },
      { path: 'signout-callback', component: SignoutRedirectCallbackComponent },
      { path: 'unauthorized', component: UnauthorizedComponent },
      { path: '', component: HomeComponent, pathMatch: 'full' },
    ])
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
