import { ICaseInfo } from "../type_defs/ICaseInfo";
import { IComment } from "../type_defs/IComment";

export const getTagEnums = async (baseAddr: string): Promise<string[]> => {
    try {
        const resp = await fetch(`${baseAddr}/api/caseEditUI/commentTags`, {
            method: 'get'
        });
        const respJSON = await resp.json() as {};
        // console.log(respJSON);
        return respJSON as string[];
    } catch (e) {
        console.log(e);
        return null;
    }
}

export const addComment = async (baseAddr: string, comment: IComment): Promise<boolean> => {
    try {
        const resp = await fetch(`${baseAddr}/api/caseEditUI/${comment.reportingCaseId}/addComment`, {
            method: 'post',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(comment)
        });
        const respJSON = await resp.json() as {};
        console.log(respJSON);
        return true
    } catch (e) {
        console.log(e);
        return false;
    }
}