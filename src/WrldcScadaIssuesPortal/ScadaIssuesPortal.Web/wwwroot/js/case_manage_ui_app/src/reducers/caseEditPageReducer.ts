import { ICaseEditPageState } from "../type_defs/ICaseEditPageState";
import { getCaseInfo } from "../server_mediators/case_data";
import * as actionTypes from '../actions/actionTypes';
import { IAction } from "../type_defs/IAction";
import { useReducer, useCallback, useEffect } from "react";

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
                return { ...state, info: action.payload };
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
            default:
                pageStateDispatch(action);
        }
    }, []); // The empty array causes this callback to only be created once per component instance

    useEffect(() => {
        document.title = `Id is ${pageState.info.id}`;
        // asyncDispatch({ type: actionTypes.setCaseInfoAction });
    }, [pageState.info.id]);

    return [pageState, asyncDispatch];
}