import { ExistingEntryDto } from './../dto/ExistingEntryDto';
import { Component, Inject, Directive, EventEmitter, Input, Output, QueryList, ViewChildren } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { DatePipe } from '@angular/common';
import { DeathEntryDto } from '../dto/DeathEntryDto';
import { DeathDateResultDto } from '../dto/DeathDateResultDto';

@Component({
  selector: 'app-death-day',
  templateUrl: './death-day.component.html',
  styleUrls: ['./death-day.component.css']
})
export class DeathDayComponent {

  scoreNumberFour: number;
  entries: DeathEntryDto[];
  rejectedEntries: ExistingEntryDto[];
  i:number;
  bsValue = new Date("1993-9-4");
  MINIMUMSCORE = 48;
  minimumscore = this.MINIMUMSCORE;
  isBusy = false;

  constructor(private http: HttpClient, private datePipe: DatePipe, @Inject('BASE_URL') private baseUrl: string) {  
  }

  onDeleteEntryClicked(entryIndex:number){    
    this.entries.splice(entryIndex, 1);
  }

  onDeleteRejectedEntryClicked(entryIndex:number){    
    this.rejectedEntries.splice(entryIndex, 1);
  }

 public getDay():number{
   return Number.parseInt(this.datePipe.transform(this.bsValue,"d"));
 }

 public getDayUrl(): string{
   return "https://en.wikipedia.org/wiki/Deaths_in_" + this.datePipe.transform(this.bsValue,"MMMM_yyyy#d");
 }

 public getScoreNumberFour():number{
  if(this.entries.length === 0){
    return -1;
  }
  return this.scoreNumberFour;
 }

 public applyWarningStyle(index:number):boolean{
  return !this.entries[index].keepExisting && (this.entries[index].wikidataItem.dateOfDeath !== this.entries[index].wikipediaArticle.dateOfDeath);
 }

 public applyExistingStyle(index:number):boolean{
  return this.entries[index].keepExisting && (this.entries[index].wikidataItem.dateOfDeath === this.entries[index].wikipediaArticle.dateOfDeath);
 }

 public applyExistingAndWarnStyle(index:number):boolean{
  return this.entries[index].keepExisting && (this.entries[index].wikidataItem.dateOfDeath !== this.entries[index].wikipediaArticle.dateOfDeath);
 }

 public applyErrorStyle(index:number):boolean{
  if(this.entries[index].wikidataItem.mannerOfDeath === null)
    return false;

   return this.entries[index].wikidataItem.mannerOfDeath.startsWith('error');
 }

 public applyWarningStyleRejectedEntries(index:number):boolean{
  return this.rejectedEntries[index].reasonKeepReject === "Death date not found in article." || this.rejectedEntries[index].reasonKeepReject === "Wikidata states a different DoD.";
}

 getImageSource(found:boolean):string{
   if(found){
    return "../../assets/ok.png";
   }
   else{
    return "../../assets/nok.png";
   }
 }

 onFetch(){    
    this.isBusy = true;
    let dateParam:string = this.datePipe.transform(this.bsValue,"yyyy-MM-dd");

    this.http.get<DeathDateResultDto>(this.baseUrl + 'wikipedia/' + dateParam + '/' + this.minimumscore).subscribe(result => {   
      
      this.scoreNumberFour = result.scoreNumberFour;
      this.entries = result.entries;  
      this.rejectedEntries = result.rejectedExistingEntries;  

      this.isBusy = false;          
    }, error => {
      this.isBusy = false;
      console.error(error);}
      );
  }

 public addDays(days : number){
    var temp = new Date(this.bsValue.getFullYear(), this.bsValue.getMonth(), this.bsValue.getDate());
    temp.setDate(temp.getDate() + days);
    this.bsValue = temp;
    this.minimumscore = this.MINIMUMSCORE;
  }
}
