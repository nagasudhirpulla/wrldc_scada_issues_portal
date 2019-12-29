import { ICaseInfo } from "./ICaseInfo";
import { IUserInfo } from "./IUserInfo";

export interface ICaseEditPageState {
    info: ICaseInfo,
    baseAddr: string,
    users: IUserInfo[]
}