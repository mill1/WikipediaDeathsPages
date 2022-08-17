import { HttpClient } from '@angular/common/http';
import { Component, Inject, Input } from '@angular/core';
import { ArticleAnomalieResultDto } from '../dto/ArticleAnomalieResultDto';

@Component({
  selector: 'app-articles-check',
  templateUrl: './articles-check.component.html',
  styleUrls: ['./articles-check.component.css']
})
export class ArticlesCheckComponent {
  
  anomalies: ArticleAnomalieResultDto[];
  checkStatus: string;
  isBusy = false;
  @Input() deathDate: Date;

  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) {     
  }

  onFetch(){    
    this.isBusy = true;
    let checkedMonth = this.deathDate.getFullYear() + '/' + (this.deathDate.getMonth()+1);
    this.checkStatus = 'Checking the corresponding articles for anomalies for month '+checkedMonth+', please hold on... ';

    let url = this.baseUrl + 'wikipedia/articleanomalies/'+checkedMonth;
    console.log('url: ' + url);
    this.http.get<ArticleAnomalieResultDto[]>(url).subscribe(result => {   
      
      this.anomalies = result;  

      this.isBusy = false;
      this.checkStatus = 'Checks complete. Result: ';
    }, error => {
      this.isBusy = false;
      console.error(error);}
      );
  }

  getImageSource():string{

    if (this.isBusy){
      return "../../assets/loading.gif"
    }
    else{
      if(this.anomalies.length === 0){ // if(allArticlesOk()): does not work. Don't know why..
        return "../../assets/ok.png";
      }
      else{
        return "../../assets/nok.png";
      }
    }
  }

  allArticlesOk(): boolean{
    return !this.isBusy && (this.anomalies === undefined || this.anomalies.length === 0);
  }
}
