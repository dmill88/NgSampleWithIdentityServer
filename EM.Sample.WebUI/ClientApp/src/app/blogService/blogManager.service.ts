import { Injectable, Inject } from '@angular/core';
import { Observable, of, throwError } from 'rxjs';
import { HttpClient, HttpHeaders, HttpErrorResponse, HttpParams } from '@angular/common/http';
import { catchError, retry, map, retryWhen } from 'rxjs/operators';
import { SelectItem, SelectItemGroup } from 'primeng/api';

import { Author, Blog, BlogPost, BlogListItem, BlogStatuses, CommentStatuses, Post, PostStatuses } from './../models/blog.models';
import { ListItemNameId } from './../models/formControl.models';
import { WebApiHelper } from './../shared/webApiHelper';
import { BlogListComponent } from '../blogList/blogList.component';

@Injectable({
  providedIn: 'root'
})
export class BlogManagerService {
  private _baseUrl: string = '/';

  constructor(
    private _http: HttpClient,
    @Inject('BASE_URL') baseUrl: string
  ) {
    console.log(`BlogManagerService constructor baseUrl = '${baseUrl}'`);
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

  public addBlog(blog: Blog, tags: string[]): Observable<Blog> {
    let body = Object.assign({}, blog, { tags: tags });
    return this._http.post<Blog>(`${this._baseUrl}api/Blog/AddBlog`, body, { headers: new HttpHeaders().set('Content-Type', 'application/json') });
  }

  public getBlogPost(id: number): Observable<BlogPost> {
    return this._http.get<BlogPost>(`${this._baseUrl}api/BlogQueries/GetBlogPost/${id}`);
  }

  public getNewBlogPost(blogId: number): Observable<BlogPost> {
    return this._http.get<BlogPost>(`${this._baseUrl}api/BlogPosts/AddBlogPost/${blogId}`);
  }

  public addBlogPost(post: BlogPost): Observable<BlogPost> {
    //let body = Object.assign({}, blogPost, { tags: tags });
    return this._http.post<BlogPost>(`${this._baseUrl}api/BlogPosts/AddBlogPost`, post, { headers: new HttpHeaders().set('Content-Type', 'application/json') });
    //return this._http.post<BlogPost>(`${this._baseUrl}api/BlogPosts/AddBlogPost`, post);
  }

  public updateBlog(blog: Blog, tags: string[]): Observable<Blog> {
    //let cpHeaders = new Headers({ 'Content-Type': 'application/json' });
    //let options = new RequestOptions({ headers: cpHeaders });
    let body = Object.assign({}, blog, { tags: tags });
    //let body = WebApiHelper.convertObjectToFormDataString(blog);
    return this._http.put<Blog>(`${this._baseUrl}api/Blog/UpdateBlog`, body, { headers: new HttpHeaders().set('Content-Type', 'application/json') });
  }

  public updateBlogPost(blogPost: BlogPost): Observable<BlogPost> {
    return this._http.put<BlogPost>(`${this._baseUrl}api/BlogPosts/UpdatePost`,
      blogPost, { headers: new HttpHeaders().set('Content-Type', 'application/json'), responseType: "json" });
  }

  public getAllBlogPosts(blogId: number): Observable<Post[]> {
    let params = new HttpParams();
    params = params.append('blogId', blogId.toString());
    return this._http.get<Post[]>(`${this._baseUrl}api/BlogQueries/GetAllBlogPosts`, { params });
  }
    
  public getBlogPosts(blogId: number, page: number = 1, pageSize: number = 10): Observable<Post[]> {
    let params = new HttpParams();
    params = params.append('blogId', blogId.toString());
    params = params.append('page', page.toString());
    params = params.append('pageSize', pageSize.toString());

    return this._http.get<Post[]>(`${this._baseUrl}api/BlogQueries/GetBlogPosts`, { params });
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
  
  //public updateOrder(order: Order): Observable<any> {
  //  let orderHeader: any = {};
  //  let xsrfToken = WebApiHelper.getRequestVerificationToken();
  //  orderHeader = Object.assign({}, orderHeader, order, {
  //    __RequestVerificationToken: xsrfToken
  //  });
  //  return this._http
  //    .post(`${this._baseUrl}Order/Edit`, WebApiHelper.convertObjectToFormDataString(orderHeader), WebApiHelper.getFormPostRequestOptions())
  //    .catch(this.handleError);
  //}

  private handleError(error: Response) {
    debugger;
    console.log(`BlogManagerService.handleError: ${error}`);
    return Observable.throw(error || 'BlogManagerService error');
  }


  //public getOrder(id: number): Observable<Order> {
  //  console.log(`Called OrderService.getOrder(${id})`);
  //  return this._http
  //    .get(`${this._baseUrl}OrderApi/GetOrder?id=${id}`)
  //    .pipe(map(response => {
  //      let order: Order = <Order>response;
  //      Object.assign(order, { StartDate: this.intl.parseDate(response['StartDate']), EndDate: this.intl.parseDate(response['EndDate']) });
  //      return order;
  //    }))
  //    .catch(this.handleError);
  //}

  //public clearOrderApprovals(orderHeaderId: number): Observable<void> {
  //  console.log(`order.service.clearAllApprovals orderHeaderId: ${orderHeaderId}`);
  //  return this._http.delete(`/OrderApi/ClearAllApprovals?orderHeaderId=${orderHeaderId}`)
  //    .catch(this.handleError);
  //}

}

