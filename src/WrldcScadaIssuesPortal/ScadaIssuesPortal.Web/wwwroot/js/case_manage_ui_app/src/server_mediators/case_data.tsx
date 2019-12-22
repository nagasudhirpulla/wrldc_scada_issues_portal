export const getCaseInfo = async (baseAddr, caseId) => {
    try {
        const resp = await fetch(`${baseAddr}/api/caseEditUI/${caseId}`, {
            method: 'get'
        });
        const respJSON = await resp.json() as {};
        console.log(respJSON);
        return respJSON;
    } catch (e) {
        console.log(e);
        return { success: false, message: `Could not retrieve case info data due to error ${JSON.stringify(e)}` };
    }
}