import { ExistingEntryDto } from './ExistingEntryDto';
import { DeathEntryDto } from "./DeathEntryDto";

export interface DeathDateResultDto{
    dateOfDeath: Date;
    scoreNumberFour: number;
    entries: DeathEntryDto[];
    rejectedExistingEntries: ExistingEntryDto[];
}