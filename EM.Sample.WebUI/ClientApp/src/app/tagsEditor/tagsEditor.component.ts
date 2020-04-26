import { Component, OnInit, OnDestroy, Input, Output } from '@angular/core';

import { SelectItem, SelectItemGroup } from 'primeng/api';
import { Button } from 'primeng/button';
import { PickList } from 'primeng/picklist';

import { ControlValidationService } from './../shared/controlValidation.service';

@Component({
  selector: 'tag-editor',
  templateUrl: './tagsEditor.component.html',
  providers: []
})
export class TagEditorComponent implements OnInit, OnDestroy {
  @Input() public tags: string[] = new Array<string>();
  @Input() public availableTags: string[] = new Array<string>();
  //protected existingTags: string[] = new Array<string>();
  public errors: string[] = new Array<string>();

  constructor() {
  }

  public ngOnInit(): void {
    console.log('TagEditorComponent.ngOnInit()');
  }

  ngOnDestroy() {
    console.log('TagEditorComponent.ngOnDestroy');
  }

}
