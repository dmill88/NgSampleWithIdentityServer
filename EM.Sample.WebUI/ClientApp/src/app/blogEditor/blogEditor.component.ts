import { Component, OnInit, OnDestroy } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { FormGroup, FormControl, FormBuilder, Validators, AbstractControl, ValidatorFn } from '@angular/forms';
import { Observable, Subscription } from 'rxjs';

import { SelectItem, SelectItemGroup } from 'primeng/api';
import { Button } from 'primeng/button';
import { InputText } from 'primeng/inputtext';
import { Spinner } from 'primeng/spinner';
import { InputTextarea } from 'primeng/inputtextarea';

import { BlogManagerService } from './../blogService/blogManager.service';
import { BlogQueriesService } from './../blogService/blogQueries.service';
import { Author, Blog, BlogStatuses, Post, PostStatuses } from './../models/blog.models';
import { ControlValidationService } from './../shared/controlValidation.service';


@Component({
  templateUrl: './blogEditor.component.html',
  providers: [FormBuilder, BlogQueriesService, BlogManagerService]
})
export class BlogEditorComponent implements OnInit, OnDestroy {
  public blog: Blog = null;
  public blogForm: FormGroup = null;
  public savedClicked: boolean = false;
  public formValidationErrors: Array<string> = new Array<string>();
  public title: string = 'Blog Editor';
  public successSaveMessage: string = '';
  public isLoaded: boolean = false;
  public saving: boolean = false;
  public authors: SelectItem[];
  public blogStatuses: SelectItem[];
  //public existingTags: SelectItem[];
  public blogTags: string[] = new Array<string>();
  public availableBlogTags: string[] = new Array<string>();

  constructor(
    private _fb: FormBuilder,
    private _route: ActivatedRoute,
    private _router: Router,
    private _blogManager: BlogManagerService,
    private _blogQueriesService: BlogQueriesService
  ) {
  }

  public ngOnInit(): void {
    console.log('BlogEditorComponent.ngOnInit()');

    this._route.params.subscribe(params => {
      const id = +params['id'];
      if (id != null) {
        this.title = id > 0 ? 'Edit Blog' : 'Add Blog';
        this.loadBlog(id);
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
    this.blogForm = this._fb.group({
      'id': [this.blog.id, Validators.required],
      'guid': [this.blog.guid],
      'blogStatusId': [this.blog.blogStatusId, [Validators.required, ControlValidationService.requiredIdValidator]],
      'displayName': [this.blog.displayName, [Validators.required, Validators.maxLength(350)]],
      'displayOrder': [this.blog.displayOrder],
      'description': [this.blog.description, [Validators.required, Validators.maxLength(2000)]],
      'name': [this.blog.name, [Validators.required, Validators.maxLength(350)]],
      'primaryAuthorId': [this.blog.primaryAuthorId, [ControlValidationService.requiredIdValidator]]
    });
  }

  public loadBlog(id: number): void {
    this._blogQueriesService.getBlog(id).subscribe(blog => {
      this.blog = blog;
      this.loadAuthorList();
      this.loadBlogStatuses();
      this.buildForm();
      this.loadBlogTags(blog.id);
      this.title = "Edit: " + blog.displayName;
    }, error => {
      this.isLoaded = true;
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

  private loadBlogTags(blogId: number): void {
    this._blogQueriesService.getBlogTags(blogId).subscribe((tags) => {
      this.blogTags = tags;
    }, (error) => {
      debugger;
      console.log(JSON.stringify(error));
      this.formValidationErrors = this.formValidationErrors.concat(ControlValidationService.parseWebApiErrors(error));
      });

    this._blogQueriesService.getUnusedBlogTags(blogId).subscribe((tags) => {
      this.availableBlogTags = tags;
    }, (error) => {
      debugger;
      console.log(JSON.stringify(error));
      this.formValidationErrors = this.formValidationErrors.concat(ControlValidationService.parseWebApiErrors(error));
    });
  }

  private loadBlogStatuses(): void {
    this.blogStatuses = this._blogQueriesService.getBlogStatusSelectList();
  }

  public get pageTitle(): string {
    let title: string = 'Edit Blog';
    if (this.blog && this.blog.id && this.isLoaded) {
      title = `Edit ${this.blog.displayName}`;
    }
    return title;
  }

  public routeToBlogAdmin() {
    this._router.navigate(['blogList']);
  }
 
  public saveBlog(): void {
    console.log("saveBlog called");
    //console.log(`Blog model: ${JSON.stringify(this.blog)}`);
    console.log(`blogForm value: ${JSON.stringify(this.blogForm.value)}`);
    this.savedClicked = true;
    this.clearMessages();
    const displayNameMap: Map<string, string> = new Map<string, string>([
      ["primaryAuthorId", "Primary Author"],
      ["displayOrder", "Display Order"],
      ["blogStatusId", "Blog Status"],
    ]);

    this.formValidationErrors = ControlValidationService.getValidationFormErrors(this.blogForm, displayNameMap);

    if (this.formValidationErrors.length === 0) {
      this.saving = true;

      let blog: Blog = <Blog>this.blogForm.value;

      if (blog.id > 0) {
        this._blogManager.updateBlog(this.blogForm.value, this.blogTags).subscribe((blog) => {
          this.blog = blog;
          this.successSaveMessage = 'Successfully updated blog';
          // Redirect to edit view
          //this._router.navigateByUrl(`Order/Edit/${data.ID}`);
        }, (error) => {
          debugger;
          console.log(JSON.stringify(error));
          this.saving = false;
          this.formValidationErrors = ControlValidationService.parseWebApiErrors(error);
        }, () => {
          this.saving = false;
        });
      } else {
        this._blogManager.addBlog(this.blogForm.value, this.blogTags).subscribe((blog) => {
          this.blog = blog;
          this.successSaveMessage = 'Successfully added blog';
          // Redirect to edit view
          //this._router.navigateByUrl(`Order/Edit/${data.ID}`);
        }, (error) => {
          debugger;
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
