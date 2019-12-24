import React, { useReducer, useCallback } from 'react';
import pageInitState from '../initial_states/caseEditPageInitState';
import { useCaseEditPageReducer } from '../reducers/caseEditPageReducer';
import * as actionTypes from '../actions/actionTypes';

// getting it from variable initialized from view bag
declare var _caseId: number;

function CaseEditPage() {
    pageInitState.info.id = _caseId;
    let [pageState, pageStateDispatch] = useCaseEditPageReducer(pageInitState);

    return (
        <>
            <div>
                <p>Id is {pageState.info.id}</p>
                <button onClick={() => pageStateDispatch({ type: actionTypes.incrementAction })}>+1</button>
                <button onClick={() => pageStateDispatch({ type: actionTypes.decrementAction })}>-1</button>
            </div>
            <div>
                <pre>{JSON.stringify(pageState, null, 4)}</pre>
            </div>
        </>
    );
}

export default CaseEditPage;