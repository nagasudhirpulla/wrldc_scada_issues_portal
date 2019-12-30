import { ICaseInfo } from "../type_defs/ICaseInfo";

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