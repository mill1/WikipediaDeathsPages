import { WikiDataItemDto } from "./WikiDataItemDto";
import { WikipediaArticleDto } from "./WikipediaArticleDto";

export interface DeathEntryDto {
  Id: string;
  wikiText: string;
  notabilityScore: number;
  referenceUrl: string;
  knownFor: string;
  keepExisting: boolean;
  wikidataItem: WikiDataItemDto;
  wikipediaArticle: WikipediaArticleDto;
}
