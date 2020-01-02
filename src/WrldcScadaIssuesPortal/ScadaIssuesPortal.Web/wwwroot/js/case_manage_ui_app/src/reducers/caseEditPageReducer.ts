import { ICaseEditPageState } from "../type_defs/ICaseEditPageState";
import { getCaseInfo, editCaseInfo } from "../server_mediators/case_data";
import { getUsers, getCurrentUser } from "../server_mediators/users";
import { getTagEnums, addComment, delComment } from "../server_mediators/comments";
import * as actionTypes from '../actions/actionTypes';
import { IAction } from "../type_defs/IAction";
import { useReducer, useCallback, useEffect } from "react";
import { IUserInfo } from "../type_defs/IUserInfo";
import { ICaseInfo } from "../type_defs/ICaseInfo";

export const useCaseEditPageReducer = (initState: ICaseEditPageState): [ICaseEditPageState, React.Dispatch<IAction>] => {
    // create the reducer function
    const reducer = (state: ICaseEditPageState, action: IAction) => {
        switch (action.type) {
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

    // set comment tag types from server
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

    // created middleware to intercept dispatch calls that require async operations
    const asyncDispatch: React.Dispatch<IAction> = useCallback(async (action) => {
        switch (action.type) {
            case actionTypes.setCaseInfoAction: {
                const caseInfo = await editCaseInfo(pageState.baseAddr, action.payload);
                pageStateDispatch({
                    type: actionTypes.setCaseInfoAction,
                    payload: caseInfo
                });
                break;
            }
            case actionTypes.addCommentAction: {
                const newCommObj = action.payload;
                console.log("newCommObj")
                console.log(newCommObj)
                const commRes = await addComment(pageState.baseAddr, newCommObj)
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
            case actionTypes.delCommentAction: {
                const commId = action.payload;
                console.log("deleting commId = ")
                console.log(commId)
                const commRes = await delComment(pageState.baseAddr, commId)
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
    }, [pageState.baseAddr, pageState.info.id]); // The empty array causes this callback to only be created once per component instance



    useEffect(() => {
        document.title = `Issue id ${pageState.info.id}`;
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