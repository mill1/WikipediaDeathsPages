import { HttpClient } from '@angular/common/http';
import { Component, Inject, OnInit } from '@angular/core';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.css']
})
export class NavMenuComponent implements OnInit {
  isExpanded = false;
  version = "1.0.0.0";

  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) {  
  }

  ngOnInit(): void {
    this.fetchAssemblyVersion();
  }

  collapse() {
    this.isExpanded = false;
  }

  toggle() {
    this.isExpanded = !this.isExpanded;
  }

  fetchAssemblyVersion(){    
    this.http.get<AssemblyProperty>(this.baseUrl + 'assembly/property/Version').subscribe(result => {   
      this.version = result.value;         
    }, error => {      
      console.error(error);}
      );
  }
}

interface AssemblyProperty {
  name: string;
  value: string;
}

