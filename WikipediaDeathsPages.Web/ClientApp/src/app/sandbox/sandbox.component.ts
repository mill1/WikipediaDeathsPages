import { Component, ElementRef, ViewChild } from '@angular/core';

@Component({
  selector: 'app-sandbox-component',
  templateUrl: './sandbox.component.html'
})
export class SandboxComponent {
  public currentCount = 0;
  @ViewChild('selectedDate', {static: false}) dateInput: ElementRef;
  //entries:

  public incrementCounter() {
    this.currentCount++;

    alert(this.hasDuplicates(["A", "A"])) // true
    alert(this.hasDuplicates(["A", "B"])) // false
  }

  hasDuplicates<T>(arr: T[]): boolean {
    return new Set(arr).size < arr.length;
  }

  onClick(){
    let selectedDate: Date = new Date(this.dateInput.nativeElement.value);
    alert(selectedDate);   
  }  
}
