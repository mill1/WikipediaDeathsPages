export interface ExistingEntryDto{
    dateOfDeath: Date;
    articleName: string;
    information: string;
    reference: string;
    referenceUrl: string;
    uri: string;
    wikiText: string;
    notabilityScore: number;
    keep: boolean;
    reasonKeepReject: string;
}