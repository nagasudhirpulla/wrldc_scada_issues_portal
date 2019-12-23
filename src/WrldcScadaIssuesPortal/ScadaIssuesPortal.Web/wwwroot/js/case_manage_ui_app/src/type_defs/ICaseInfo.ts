import { ICaseItem } from "./ICaseItem";
import { IConcernedAgency } from "./IConcernedAgency";
import { IComment } from "./IComment";
export interface ICaseInfo {
    id: number;
    caseItems: ICaseItem[];
    concernedAgencies: IConcernedAgency[];
    comments: IComment[];
    downTime: Date;
    resolutionTime: Date;
    createdAt: Date;
    updatedAt: Date;
    createdById: string;
}
