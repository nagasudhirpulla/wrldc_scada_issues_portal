import { ICaseEditPageState } from "../type_defs/ICaseEditPageState";
import { getCaseInfo } from "../server_mediators/case_data";
import { getUsers, getCurrentUser } from "../server_mediators/users";
import { getTagEnums, addComment } from "../server_mediators/comments";
import * as actionTypes from '../actions/actionTypes';
import { IAction } from "../type_defs/IAction";
import { useReducer, useCallback, useEffect } from "react";
import { IUserInfo } from "../type_defs/IUserInfo";
import { ICaseInfo } from "../type_defs/ICaseInfo";
import { IComment } from "../type_defs/IComment";

export const useCaseEditPageReducer = (initState: ICaseEditPageState): [ICaseEditPageState, React.Dispatch<IAction>] => {
    // create the reducer function
    const reducer = (state: ICaseEditPageState, action: IAction) => {
        switch (action.type) {
            case actionTypes.incrementAction:
                return {
                    ...state,
                    info: {
                        ...state.info,
                        id: state.info.id + 1
                    }
                };
            case actionTypes.decrementAction:
                return {
                    ...state,
                    info: {
                        ...state.info,
                        id: state.info.id - 1
                    }
                };
            case actionTypes.setCaseInfoAction:
                return { ...state, info: action.payload as ICaseInfo };
            case actionTypes.setUsersAction:
                return { ...state, users: action.payload as IUserInfo[] };
            case actionTypes.setCommentTagTypesAction:
                return { ...state, commentTagTypes: action.payload as string[] };
            default:
                throw new Error();
            // return state also works
        }
    }

    // create the reducer hook
    let [pageState, pageStateDispatch]: [ICaseEditPageState, React.Dispatch<IAction>] = useReducer(reducer, initState)

    // created middleware to intercept dispatch calls that require async operations
    const asyncDispatch: React.Dispatch<IAction> = useCallback(async (action) => {
        switch (action.type) {
            case actionTypes.setCaseInfoAction: {
                const caseInfo = await getCaseInfo(pageState.baseAddr, pageState.info.id);
                pageStateDispatch({
                    type: actionTypes.setCaseInfoAction,
                    payload: caseInfo
                });
                break;
            }
            case actionTypes.addCommentAction: {
                const data = action.payload;
                const commObj: IComment = {
                    reportingCaseId: pageState.info.id,
                    comment: data.comm,
                    tag: pageState.commentTagTypes.findIndex(ct => ct === data.commTag),
                    created: "",
                    createdById: "",
                    id: -1
                }
                const commRes = await addComment(pageState.baseAddr, commObj)
                if (commRes == true) {
                    // we successfully created a comment!
                    // reload the whole case Object
                    const caseInfo = await getCaseInfo(pageState.baseAddr, pageState.info.id);
                    pageStateDispatch({
                        type: actionTypes.setCaseInfoAction,
                        payload: caseInfo
                    });
                }                
                break;
            }
            default:
                pageStateDispatch(action);
        }
    }, []); // The empty array causes this callback to only be created once per component instance

    useEffect(() => {
        (async function () {
            const users = await getUsers(pageState.baseAddr);
            pageStateDispatch({
                type: actionTypes.setUsersAction,
                payload: users
            });
            const commTagTypes = await getTagEnums(pageState.baseAddr);
            pageStateDispatch({
                type: actionTypes.setCommentTagTypesAction,
                payload: commTagTypes
            });
        })();
    }, []);

    useEffect(() => {
        document.title = `Id is ${pageState.info.id}`;
        (async function () {
            const caseInfo = await getCaseInfo(pageState.baseAddr, pageState.info.id);
            pageStateDispatch({
                type: actionTypes.setCaseInfoAction,
                payload: caseInfo
            });
        })();
        // asyncDispatch({ type: actionTypes.setCaseInfoAction });
    }, [pageState.info.id]);

    return [pageState, asyncDispatch];
}