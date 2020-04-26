
export class ListItemNameId {
  public name: string;
  public id: number;
  public selected?: boolean;

  constructor(name: string, id: number) {
    this.name = name;
    this.id = id;
  }
}

export class SelectListItem {
  public text: string;
  public value: any;
  public selected: boolean;
}

export class ApiResult {
  constructor(result: boolean = false, errorMessages: Array<string> = null) {
    this.result = result;
    this.errorMessages = errorMessages;
  }

  public result: boolean;
  public errorMessages: Array<string>;
}



