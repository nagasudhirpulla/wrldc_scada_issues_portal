import { ICaseInfo } from "../type_defs/ICaseInfo";
import { IAddAttachmentPayload } from "../type_defs/IAddAttachmentPayload";

export const getCaseInfo = async (baseAddr: string, caseId: number): Promise<ICaseInfo> => {
    try {
        const resp = await fetch(`${baseAddr}/api/caseEditUI/${caseId}`, {
            method: 'get'
        });
        const respJSON = await resp.json() as {};
        // console.log(respJSON);
        return respJSON as ICaseInfo;
    } catch (e) {
        console.log(e);
        return null;
    }
}

export const editCaseInfo = async (baseAddr: string, caseObj: ICaseInfo): Promise<ICaseInfo> => {
    try {
        const resp = await fetch(`${baseAddr}/api/caseEditUI/${caseObj.id}`, {
            method: 'post',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(caseObj)
        });
        const respJSON = await resp.json() as {};
        console.log(respJSON);
        return respJSON as ICaseInfo;
    } catch (e) {
        console.log(e);
        return null;
    }
}

export const delAttachment = async (baseAddr: string, caseId: number): Promise<{ success: boolean, payload: any }> => {
    try {
        const resp = await fetch(`${baseAddr}/api/caseEditUI/attachment/${caseId}`, {
            method: 'delete',
            headers: {
                'Content-Type': 'application/json'
            },
        });
        if (resp.status != 200) {
            throw Error(await resp.text())
        }
        console.log(`attachment successfully deleted for case Id = ${caseId}`);
        return { success: true, payload: {} }
    } catch (e) {
        console.log(e);
        return { success: false, payload: e.message };
    }
}

export const addAttachment = async (baseAddr: string, formPayload: IAddAttachmentPayload): Promise<{ success: boolean, payload: any }> => {
    try {
        let formData = new FormData()
        formData.append("id", formPayload.id)
        formData.append("caseAttachment", formPayload.caseAttachment)
        const resp = await fetch(`${baseAddr}/api/caseEditUI/attachment`, {
            method: 'post',
            body: formData
        });
        if (resp.status != 200) {
            throw Error(await resp.text())
        }
        console.log(`attachment successfully added`);
        return { success: true, payload: {} }
    } catch (e) {
        console.log(e);
        return { success: false, payload: e.message };
    }
}