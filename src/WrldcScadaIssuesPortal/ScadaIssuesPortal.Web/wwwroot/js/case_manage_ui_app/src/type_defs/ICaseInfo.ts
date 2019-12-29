import { ICaseItem } from "./ICaseItem";
import { IConcernedAgency } from "./IConcernedAgency";
import { IComment } from "./IComment";
export interface ICaseInfo {
    id: number;
    caseItems: ICaseItem[];
    concernedAgencies: IConcernedAgency[];
    comments: IComment[];
    downTime: string;
    resolutionTime: string;
    createdAt: string;
    updatedAt: string;
    createdById: string;
}
