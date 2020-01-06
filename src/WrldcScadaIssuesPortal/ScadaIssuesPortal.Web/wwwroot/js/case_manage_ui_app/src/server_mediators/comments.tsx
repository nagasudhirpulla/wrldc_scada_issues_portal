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

export const addComment = async (baseAddr: string, comment: IComment): Promise<{ success: boolean, payload: any }> => {
    try {
        const resp = await fetch(`${baseAddr}/api/caseEditUI/${comment.reportingCaseId}/addComment`, {
            method: 'post',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(comment)
        });
        if (resp.status != 200) {
            throw Error(await resp.text())
        }
        const respJSON = await resp.json() as IComment;
        console.log("new comment response");
        console.log(respJSON);
        return { success: true, payload: {} }
    } catch (e) {
        console.log(e);
        return { success: false, payload: e.message };
    }
}

export const delComment = async (baseAddr: string, commId: number): Promise<{ success: boolean, payload: any }> => {
    try {
        const resp = await fetch(`${baseAddr}/api/caseEditUI/deleteComment/${commId}`, {
            method: 'delete',
            headers: {
                'Content-Type': 'application/json'
            },
        });
        if (resp.status != 200) {
            throw Error(await resp.text())
        }
        const respJSON = await resp.json() as IComment;
        console.log("delete comment response");
        console.log(respJSON);
        return { success: true, payload: {} }
    } catch (e) {
        console.log(e);
        return { success: false, payload: e.message };
    }
}