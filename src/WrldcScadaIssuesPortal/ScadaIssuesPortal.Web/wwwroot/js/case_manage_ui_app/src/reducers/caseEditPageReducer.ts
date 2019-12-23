import { ICaseEditPageState } from "../type_defs/ICaseEditPageState";
import { getCaseInfo } from "../server_mediators/case_data";

export default (state: ICaseEditPageState, action): ICaseEditPageState => {
    switch (action.type) {
        case 'increment':
            return {
                ...state,
                info: {
                    ...state.info,
                    id: state.info.id + 1
                }
            };
        //case 'init':
        //    const caseInfo = getCaseInfo(state.baseAddr, state.info.id).then;
        //    return { ...state, info: caseInfo };
        default:
            throw new Error();
    }
}