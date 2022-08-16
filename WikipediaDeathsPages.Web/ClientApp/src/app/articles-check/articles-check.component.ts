import { HttpClient } from '@angular/common/http';
import { Component, Inject } from '@angular/core';
import { DeathDateResultDto } from '../dto/DeathDateResultDto';
import { DeathEntryDto } from '../dto/DeathEntryDto';

@Component({
  selector: 'app-articles-check',
  templateUrl: './articles-check.component.html',
  styleUrls: ['./articles-check.component.css']
})
export class ArticlesCheckComponent {
  
  entries: DeathEntryDto[];
  checkStatus: string;
  isBusy = false;

  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) {     
  }


  onFetch(){    
    this.isBusy = true;
    this.checkStatus = 'Checking reference websites, please hold on...';    

    this.http.get<DeathDateResultDto>(this.baseUrl + 'wikipedia/1992-6-2/48').subscribe(result => {   
      
      this.entries = result.entries;  

      this.isBusy = false;
      this.checkStatus = 'Check complete';          
    }, error => {
      this.isBusy = false;
      console.error(error);}
      );
  }

  allArticlesOk(): boolean{    
    console.log(this.entries)
    return !this.isBusy && (this.entries === undefined || this.entries.length === 3);
  }

}
