import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpClient, HttpParams, HttpHeaders, HttpResponse } from '@angular/common/http';
import { ListItemNameId } from '../models/formControl.models';
import { SelectItem } from 'primeng/api';

export class WebApiHelper {

  public static getFormPostRequestOptions(): any {
    const headers = new HttpHeaders({ 'Content-Type': 'application/x-www-form-urlencoded' });
    return { headers: headers };
  };

  public static getRequestVerificationToken(): string {
    let token: string = '';
    debugger;
    let values = document.getElementsByName("__RequestVerificationToken");
    if (values.length > 0) {
      token = values[0].getAttribute('value');
    }
    return token;
  }

  public static convertListItemNameIDToNgPrimeSelectItemArray(items: ListItemNameId[]): SelectItem[] {
    let list = new Array<SelectItem>();
    let listData: ListItemNameId[] = items;
    for (let item of listData) {
      list.push({ label: item.name, value: item.id });
    }
    return list;
  }

  public static getFileNameFromResponseContentDisposition(res: Response): string {
    let filename: string = '';
    if (res.headers) {
      const contentDisposition = res.headers.get('content-disposition') || '';
      const matches = /filename=([^;]+)/ig.exec(contentDisposition);
      filename = (matches[1] || 'untitled').trim();
    }
    return filename;
  };

  public static getNumberParameterValue(value: any): number {
    let id: number = null;
    if (value && typeof value === "object") {
      if (value && value.ID) {
        id = value.ID;
      } else if (value && value.Value) {
        id = value.Value;
      }
    }
    else if (typeof value === "number") {
      id = value;
    }
    return id;
  }

  public static convertObjectToFormDataString(obj: any, parentKey: string = '', depth: number = 0): string {
    // console.log(`called convertObjectToFormDataString parentKey ${parentKey} depth ${depth}`);
    ///let str = querystring.stringify(obj);    // Doesn't work with objects that have Date properties.
    //return $.param(obj);    // Doesn't work with objects that have Date properties.
    let returnData = '';
    let count: number = 0;
    let item: any;
    let tempVal: string = '';
    parentKey = parentKey.trim();

    for (let propName in obj) {
      if (obj.hasOwnProperty(propName)) {
        item = obj[propName];
        if (item instanceof Array) {
          for (let i = 0; i < item.length; i++) {
            if (item[i] instanceof Object) {
              returnData += this.convertObjectToFormDataString(item[i], `${parentKey}${propName}[${i}].`, depth + 1);
              if (!returnData.endsWith('&')) {
                returnData += '&'
              }
            } else {
              if (parentKey.length > 0) {
                returnData += parentKey;
              }
              returnData += propName + '=' + this.encodeForFormString(item[i]);
              if (!returnData.endsWith('&')) {
                returnData += '&'
              }
            }
          }
        }
        else if (item instanceof Date) {
          if (parentKey.length > 0) {
            returnData += parentKey;
          }
          returnData += propName + '=' + this.encodeForFormString(item);
        }
        else if (item instanceof Object && depth < 10) {
          returnData += this.convertObjectToFormDataString(item, `${parentKey}${propName}.`, depth + 1);
          if (!returnData.endsWith('&')) {
            returnData += '&'
          }
        }
        else {
          if (parentKey.length > 0) {
            returnData += parentKey;
          }
          returnData += propName + '=' + this.encodeForFormString(item);
        }

        if (returnData.length > 0 && !returnData.endsWith('&')) returnData += '&';
      }
    }
    returnData = returnData.substring(0, returnData.lastIndexOf('&'));
    //console.log("convertObjectToFormDataString: " + returnData);
    return returnData;
  }

  public static convertSimpleObjectToFormDataString(obj: any): string {
    ///let str = querystring.stringify(obj);    // Doesn't work with objects that have Date properties.
    //return $.param(obj);    // Doesn't work with objects that have Date properties.
    let returnData = '';
    let count = 0;
    for (let propName in obj) {
      if (obj.hasOwnProperty(propName)) {
        //debugger;
        if (count == 0) {
          returnData += propName + '=' + this.encodeForFormString(obj[propName]);
        } else {
          returnData += '&' + propName + '=' + this.encodeForFormString(obj[propName]);
        }
        count++;
      }
    }
    console.log("convertObjectToFormDataString: " + returnData);
    return returnData;
  }

  private static encodeForFormString(val): string {
    let strVal: string;
    if (val instanceof Date) {
      strVal = JSON.stringify(val);
      console.log("encodeForFormString Date type: " + strVal);
    }
    else if (val instanceof Boolean) {
      strVal = val.toString();
      console.log("encodeForFormString Boolean type: " + strVal);
    }
    else if (val === null || val === undefined) {
      strVal = '';
    }
    else {
      //strVal = val;
      strVal = encodeURIComponent(val);
    }
    return strVal;
  }


}
