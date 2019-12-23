import { IUserInfo } from "../type_defs/IUserInfo";

export const getUsers = async (baseAddr): Promise<IUserInfo[]> => {
    try {
        const resp = await fetch(`${baseAddr}/api/users`, {
            method: 'get'
        });
        const respJSON = await resp.json() as {};
        // console.log(respJSON);
        return respJSON as IUserInfo[];
    } catch (e) {
        console.log(e);
        return null;
    }
}

export const getCurrentUser = async (baseAddr): Promise<IUserInfo> => {
    try {
        const resp = await fetch(`${baseAddr}/api/currentUser`, {
            method: 'get'
        });
        const respJSON = await resp.json() as {};
        // console.log(respJSON);
        return respJSON as IUserInfo;
    } catch (e) {
        console.log(e);
        return null;
    }
}