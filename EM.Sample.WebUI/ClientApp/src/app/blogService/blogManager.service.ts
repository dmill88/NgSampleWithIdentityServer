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

  public addBlog(blog: Blog, tags: string[]): Observable<Blog> {
    let body = Object.assign({}, blog, { tags: tags });
    return this._http.post<Blog>(`${this._baseUrl}api/Blog/AddBlog`, body, { headers: new HttpHeaders().set('Content-Type', 'application/json') });
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
    let body = Object.assign({}, blog, { tags: tags });
    return this._http.put<Blog>(`${this._baseUrl}api/Blog/UpdateBlog`, body, { headers: new HttpHeaders().set('Content-Type', 'application/json') });
  }

  public updateBlogPost(blogPost: BlogPost): Observable<BlogPost> {
    return this._http.put<BlogPost>(`${this._baseUrl}api/BlogPosts/UpdatePost`,
      blogPost, { headers: new HttpHeaders().set('Content-Type', 'application/json'), responseType: "json" });
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

