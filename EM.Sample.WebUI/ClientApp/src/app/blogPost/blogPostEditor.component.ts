import { Component, OnInit, OnDestroy } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { FormGroup, FormControl, FormBuilder, Validators, AbstractControl, ValidatorFn } from '@angular/forms';
import { Observable, Subscription } from 'rxjs';

import { SelectItem, SelectItemGroup } from 'primeng/api';
import { Button } from 'primeng/button';
import { InputText } from 'primeng/inputtext';
import { Spinner } from 'primeng/spinner';
import { InputTextarea } from 'primeng/inputtextarea';
import { Editor } from 'primeng/editor';

import { BlogManagerService } from './../blogService/blogManager.service'
import { BlogQueriesService } from './../blogService/blogQueries.service';
import { Author, Blog, BlogPost, BlogStatuses, Post, PostStatuses } from './../models/blog.models';
import { ControlValidationService } from './../shared/controlValidation.service';


@Component({
  templateUrl: './blogPostEditor.component.html',
  styleUrls: ['./blogPostEditor.component.scss'],
  providers: [FormBuilder, BlogManagerService, BlogQueriesService]
})
export class BlogPostEditorComponent implements OnInit, OnDestroy {
  public blog: Blog = null;
  public blogPost: BlogPost = null;
  public blogPostForm: FormGroup = null;
  public savedClicked: boolean = false;
  public formValidationErrors: Array<string> = new Array<string>();
  public title: string = 'Blog Post';
  public successSaveMessage: string = '';
  public isLoaded: boolean = false;
  public saving: boolean = false;
  public authors: SelectItem[];
  public postStatuses: SelectItem[];
  public commentStatuses: SelectItem[];
  public postTags: string[] = new Array<string>();
  public availableTags: string[] = new Array<string>();

  constructor(
    private _fb: FormBuilder,
    private _route: ActivatedRoute,
    private _blogManager: BlogManagerService,
    private _blogQueriesService: BlogQueriesService) {
  }

  public ngOnInit(): void {
    console.log('BlogEditorComponent.ngOnInit()');

    this._route.params.subscribe(params => {
      const id = +params['id'];
      const blogId = +params['blogId'];
      if (id != null && id > 0) {
        this.title = 'Edit Blog Post';
        this.loadBlogPost(id);

        //if (id === 0 && blogId > 0) {
        //  this.getNewBlogPostModel(blogId);
        //} else if (id > 0) {
        //} else {
        //  debugger;
        //  // TODO: Add error message
        //}
      } else if (blogId !== null && blogId > 0) {
        this.getNewBlogPostModel(blogId);
      }
    }, error => {
      console.log(JSON.stringify(error));
    });
  }

  ngOnDestroy() {
    console.log('BlogEditorComponent.ngOnDestroy');
  }

  private buildForm(): void {
    console.log("BlogEditorComponent.buildForm");
    this.blogPostForm = this._fb.group({
      'id': [this.blogPost.id, Validators.required],
      'guid': [this.blogPost.guid],
      'blogId': [this.blogPost.blogId, ControlValidationService.requiredIdValidator],
      'commentStatusId': [this.blogPost.commentStatusId, ControlValidationService.requiredIdValidator],
      'excerpt': [this.blogPost.excerpt, [Validators.required, Validators.maxLength(1000)]],
      'postContent': [this.blogPost.postContent],
      'postStatusId': [this.blogPost.postStatusId, ControlValidationService.requiredIdValidator],
      'primaryAuthorId': [this.blogPost.primaryAuthorId, ControlValidationService.requiredIdValidator],
      'title': [this.blogPost.title, [Validators.required, Validators.maxLength(450)]],
      'tags': [this.postTags]
    });
  }

  public getNewBlogPostModel(blogId: number): void {
    this._blogManager.getNewBlogPost(blogId).subscribe(blogPost => {
      this.blogPost = blogPost;
      this.loadAuthorList();
      this.postStatuses = this._blogQueriesService.getPostStatuses();
      this.commentStatuses = this._blogQueriesService.getCommentStatuses();
      this.buildForm();
      this.loadBlogPostDefaultTags(blogPost.blogId);
    }, error => {
      //debugger;
      this.isLoaded = true;
      console.log(JSON.stringify(error));
      this.formValidationErrors = ControlValidationService.parseWebApiErrors(error);
    }, () => {
      this.isLoaded = true;
    });
  }

  public loadBlogPost(id: number): void {
    this._blogQueriesService.getBlogPost(id).subscribe(blogPost => {
      this.blogPost = blogPost;
      this.postTags = blogPost.tags;
      this.loadAuthorList();
      this.postStatuses = this._blogQueriesService.getPostStatuses();
      this.commentStatuses = this._blogQueriesService.getCommentStatuses();
      this.buildForm();
      this.getUnusedPostTags(blogPost.id);
      this.title = "Edit: " + blogPost.title;
    }, error => {
      this.isLoaded = true;
      debugger;
      console.log(JSON.stringify(error));
      this.formValidationErrors = ControlValidationService.parseWebApiErrors(error);
    }, () => {
      this.isLoaded = true;
    });
  }

  private loadAuthorList(): void {
    this._blogQueriesService.getAuthorsSelectList().subscribe((authors) => {
      this.authors = authors;
    }, (error) => {
      debugger;
      console.log(JSON.stringify(error));
      this.formValidationErrors = this.formValidationErrors.concat(ControlValidationService.parseWebApiErrors(error));
    });
  }

  private loadBlogPostDefaultTags(blogId: number): void {
    this._blogQueriesService.getBlogTags(blogId).subscribe((tags) => {
      this.postTags = tags;
    }, (error) => {
      debugger;
      console.log(JSON.stringify(error));
      this.formValidationErrors = this.formValidationErrors.concat(ControlValidationService.parseWebApiErrors(error));
    });

    this._blogQueriesService.getUnusedBlogTags(blogId).subscribe((tags) => {
      this.availableTags = tags;
    }, (error) => {
      debugger;
      console.log(JSON.stringify(error));
      this.formValidationErrors = this.formValidationErrors.concat(ControlValidationService.parseWebApiErrors(error));
    });
  }

  private getUnusedPostTags(id: number): void {
    //this._blogQueriesService.getPostTags(id).subscribe((tags) => {
    //  this.postTags = tags;
    //}, (error) => {
    //  debugger;
    //  console.log(JSON.stringify(error));
    //  this.formValidationErrors = this.formValidationErrors.concat(ControlValidationService.parseWebApiErrors(error));
    //});

    this._blogQueriesService.getUnusedPostTags(id).subscribe((tags) => {
      this.availableTags = tags;
    }, (error) => {
      debugger;
      console.log(JSON.stringify(error));
      this.formValidationErrors = this.formValidationErrors.concat(ControlValidationService.parseWebApiErrors(error));
    });
  }

  public get pageTitle(): string {
    let title = 'Edit Blog';
    if (this.blogPost && this.blogPost.title && this.isLoaded) {
      if (this.blogPost.id) {
        title = `Edit ${this.blogPost.title}`;
      } else if (this.blogPost.blogId)
        title = `Add Blog Post`;
    }
    return title;
  }

  public saveBlogPost(): void {
    console.log("saveBlogPost called");
    //console.log(`Blog model: ${JSON.stringify(this.blog)}`);
    console.log(`blogForm value: ${JSON.stringify(this.blogPostForm.value)}`);
    this.savedClicked = true;
    this.clearMessages();
    const displayNameMap: Map<string, string> = new Map<string, string>([
      ["primaryAuthorId", "Primary Author"],
      ["displayOrder", "Display Order"],
      ["blogStatusId", "Blog Status"],
    ]);

    this.formValidationErrors = ControlValidationService.getValidationFormErrors(this.blogPostForm, displayNameMap);

    if (this.formValidationErrors.length === 0) {
      this.saving = true;

      const post: Post = this.blogPostForm.value as Post;
      //for (let tag of this.postTags) {
      //  //post.tags.push(tag);
      //}
      //post.tags = this.postTags;

      if (post.id > 0) {
        this._blogManager.updateBlogPost(this.blogPostForm.value).subscribe((blogPost) => {
          this.blogPost = blogPost;
          this.successSaveMessage = 'Successfully updated blog post';
          // Redirect to edit view
          // this._router.navigateByUrl(`Order/Edit/${data.ID}`);
        }, (error) => {
          //debugger;
          console.log(JSON.stringify(error));
          this.saving = false;
          this.formValidationErrors = ControlValidationService.parseWebApiErrors(error);
        }, () => {
          this.saving = false;
        });
      } else {
        this._blogManager.addBlogPost(this.blogPostForm.value).subscribe((blogPost) => {
          this.blogPost = blogPost;
          this.successSaveMessage = 'Successfully added blog post';
          // Redirect to edit view
          // this._router.navigateByUrl(`Order/Edit/${data.ID}`);
        }, (error) => {
          //debugger;
          console.log(JSON.stringify(error));
          this.saving = false;
          this.formValidationErrors = ControlValidationService.parseWebApiErrors(error);
        }, () => {
          this.saving = false;
        });
      }
    }
  }

  private clearMessages(): void {
    this.successSaveMessage = '';
    this.formValidationErrors.length = 0;
  }


}
