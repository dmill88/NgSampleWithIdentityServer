import { Component, OnInit, OnDestroy, Input } from '@angular/core';

@Component({
  selector: 'error-list',
  templateUrl: './errorList.component.html',
  providers: []
})
export class ErrorListComponent implements OnInit, OnDestroy {
  @Input() public errors: string[] = new Array<string>();
  @Input() public showErrorTitle: boolean = true;
  @Input() public errorTitle: string = "Errors";

  constructor() {
  }

  public ngOnInit(): void {
    console.log('ErrorListComponent.ngOnInit()');
  }

  ngOnDestroy() {
    console.log('ErrorListComponent.ngOnDestroy');
  }

}
