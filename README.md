# WikipediaDeathsPages
Webapplication focussed on the content of Wikipedia deaths list articles.

### Personal website
The Wikipedia Deaths Pages assists me in my endeavour to improve the quality of the [Deaths per month articles](https://en.wikipedia.org/wiki/Lists_of_deaths_by_year) on Wikipedia.
Per selected day it generates the wikitext which can be pasted in the day subsection of a specific list article.
During that process a lot happens: 
the source of people who died on a specific date is [this Wikidata query](https://www.wikidata.org/wiki/Wikidata:Request_a_query/Archive/2021/12#Minor_issue_regarding_Humans_per_DoD_query).
The results are filtered based on a notability value before being displayed on the site. 

In addition existing entries regarding the date of death are checked for errors and cross referenced with Wikidata. 
Also, references (citations) regarding the date of death of the deceased are generated automatically based on the Wikidata statements on it. 
Various other websites are also checked automatically as sources for references (for instance [basketball-reference.com](https://www.basketball-reference.com/), 
[olympedia.org](http://www.olympedia.org/) and [worldfootball.net](https://www.worldfootball.net/)).

This application contains a lot more functionality like the ability to generate entry descriptions from the corresponding Wikipedia page of the decease (see code). 
It it is safe to say that the site sped up the process of cleaning up the Wikipedia Deaths Pages considerably. 
The process would otherwise be too time-consuming, tedious and error-prone.

![Screenshot-WDP](https://github.com/mill1/WikipediaDeathsPages/assets/30286933/dc519e03-f401-4140-8aed-1098ded147e4)
