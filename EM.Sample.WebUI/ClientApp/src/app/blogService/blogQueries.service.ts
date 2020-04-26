import { Injectable, Inject } from '@angular/core';
import { Observable, of, throwError } from 'rxjs';
import { HttpClient, HttpHeaders, HttpErrorResponse, HttpParams } from '@angular/common/http';
import { map } from 'rxjs/operators';
import { SelectItem, SelectItemGroup } from 'primeng/api';

import { Author, Blog, BlogPost, BlogListItem, BlogStatuses, CommentStatuses, Post, PostStatuses } from './../models/blog.models';
import { ListItemNameId } from './../models/formControl.models';
import { PagedResult } from './../models/pagedResult.model';
import { WebApiHelper } from './../shared/webApiHelper';
import { BlogListComponent } from '../blogList/blogList.component';

@Injectable({
  providedIn: 'root'
})
export class BlogQueriesService {
  private _baseUrl: string = '/';

  constructor(
    private _http: HttpClient,
    @Inject('BASE_URL') baseUrl: string
  ) {
    console.log(`BlogQueriesService constructor baseUrl = '${baseUrl}'`);
    this._baseUrl = baseUrl;
  }

  public getAuthorsSelectList(): Observable<Array<SelectItem>> {
    return this._http.get(`${this._baseUrl}api/AuthorQueries/GetAuthors`)
      .pipe(map(response => {
        let list = new Array<SelectItem>();
        let listData: Author[] = <Author[]>response;
        for (let item of listData) {
          list.push({ label: `${item.firstName} ${item.lastName}`, value: item.id });
        }
        return list;
      }));
  }

  public getBlogStatusSelectList(): Array<SelectItem> {
    let list = new Array<SelectItem>();
    list.push({ label: 'Draft', value: <number>BlogStatuses.Draft });
    list.push({ label: 'Review', value: <number>BlogStatuses.Review });
    list.push({ label: 'Published', value: <number>BlogStatuses.Published });
    list.push({ label: 'Archived', value: <number>BlogStatuses.Archived });
    return list;
  }

  public getPostStatuses(): Array<SelectItem> {
    let list = new Array<SelectItem>();
    list.push({ label: 'Draft', value: <number>PostStatuses.Draft });
    list.push({ label: 'Review', value: <number>PostStatuses.Review });
    list.push({ label: 'Published', value: <number>PostStatuses.Published });
    list.push({ label: 'Archived', value: <number>PostStatuses.Archived });
    return list;
  }

  public getCommentStatuses(): Array<SelectItem> {
    let list = new Array<SelectItem>();
    list.push({ label: 'Anonymous', value: <number>CommentStatuses.Anonymous });
    list.push({ label: 'Disabled', value: <number>CommentStatuses.Disabled });
    list.push({ label: 'Locked', value: <number>CommentStatuses.Locked });
    list.push({ label: 'MemberOnly', value: <number>CommentStatuses.MemberOnly });
    return list;
  }

  public getBlogs(status: BlogStatuses = BlogStatuses.Published, primaryAuthorId: number = null): Observable<Array<BlogListItem>> {
    console.log('BlogManagerService.getBlogs');
    let url = `${this._baseUrl}api/BlogQueries/GetBlogs`;
    let params = new HttpParams();
    let statusId: number = <number>status;
    params = params.append('status', statusId.toString());
    if (primaryAuthorId) {
      params = params.append('primaryAuthorId', primaryAuthorId.toString());
    }
    return this._http.get<Array<BlogListItem>>(url, { params });
  }

  public getBlog(id: number): Observable<Blog> {
    return this._http.get<Blog>(`${this._baseUrl}api/BlogQueries/GetBlog/${id}`);
  }

  public getBlogPost(id: number): Observable<BlogPost> {
    return this._http.get<BlogPost>(`${this._baseUrl}api/BlogQueries/GetBlogPost/${id}`);
  }

  public getNewBlogPost(blogId: number): Observable<BlogPost> {
    return this._http.get<BlogPost>(`${this._baseUrl}api/BlogPosts/AddBlogPost/${blogId}`);
  }

  public getAllBlogPosts(blogId: number): Observable<Post[]> {
    let params = new HttpParams();
    params = params.append('blogId', blogId.toString());
    return this._http.get<Post[]>(`${this._baseUrl}api/BlogQueries/GetAllBlogPosts`, { params });
  }

  public getBlogPosts(blogId: number, skip: number = 0, take: number = 10): Observable<PagedResult> {
    console.log("getBlogPosts", blogId, skip, take);
    let filter = { blogId: blogId, skip: skip, take: take };
    return this._http.post<PagedResult>(`${this._baseUrl}api/BlogQueries/GetBlogPosts`, filter);
  }

  public getTags(): Observable<SelectItem[]> {
    return this._http.get<ListItemNameId[]>(`${this._baseUrl}api/BlogQueries/GetTags`).pipe(map(response => {
      let list = WebApiHelper.convertListItemNameIDToNgPrimeSelectItemArray(response);
      return list;
    }));
  }

  public getTagsWithNameValue(): Observable<SelectItem[]> {
    return this._http.get<ListItemNameId[]>(`${this._baseUrl}api/BlogQueries/GetTags`).pipe(map(response => {
      let list = new Array<SelectItem>();
      let listData: ListItemNameId[] = response;
      for (let item of listData) {
        list.push({ label: item.name, value: item.name });
      }
      return list;
    }));
  }

  public getBlogTags(id: number): Observable<string[]> {
    return this._http.get<ListItemNameId[]>(`${this._baseUrl}api/BlogQueries/GetBlogTags/${id}`).pipe(map(response => {
      let list = new Array<string>();
      let listData: ListItemNameId[] = response;
      for (let item of listData) {
        list.push(item.name);
      }
      return list;
    }));
  }

  public getUnusedBlogTags(id: number): Observable<string[]> {
    return this._http.get<ListItemNameId[]>(`${this._baseUrl}api/BlogQueries/GetUnusedBlogTags/${id}`).pipe(map(response => {
      let list = new Array<string>();
      let listData: ListItemNameId[] = response;
      for (let item of listData) {
        list.push(item.name);
      }
      return list;
    }));
  }

  public getPostTags(id: number): Observable<string[]> {
    return this._http.get<ListItemNameId[]>(`${this._baseUrl}api/BlogQueries/GetPostTags/${id}`).pipe(map(response => {
      let list = new Array<string>();
      let listData: ListItemNameId[] = response;
      for (let item of listData) {
        list.push(item.name);
      }
      return list;
    }));
  }

  public getUnusedPostTags(id: number): Observable<string[]> {
    return this._http.get<ListItemNameId[]>(`${this._baseUrl}api/BlogQueries/GetUnusedPostTags/${id}`).pipe(map(response => {
      let list = new Array<string>();
      let listData: ListItemNameId[] = response;
      for (let item of listData) {
        list.push(item.name);
      }
      return list;
    }));
  }

  private handleError(error: Response) {
    debugger;
    console.log(`BlogQueriesService.handleError: ${error}`);
    return Observable.throw(error || 'BlogManagerService error');
  }

}

