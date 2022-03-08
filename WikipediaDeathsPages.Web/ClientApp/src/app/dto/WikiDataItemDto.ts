export interface WikiDataItemDto {
  articleName: string;
  label: string;
  description: string;
  uri: string;
  siteLinksCount: number;
  dateOfBirth: Date;
  dateOfDeath: Date;
  dateOfDeathRefs: string;
  causeOfDeath: string;
  mannerOfDeath: string;
}
