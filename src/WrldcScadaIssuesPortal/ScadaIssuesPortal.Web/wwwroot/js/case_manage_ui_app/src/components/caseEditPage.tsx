import React, { useReducer } from 'react';
import pageInitState from '../initial_states/caseEditPageInitState';
import CaseEditPageReducer from '../reducers/caseEditPageReducer';

// getting it from variable initialized from view bag
declare var _caseId: number;

function CaseEditPage() {
    pageInitState.info.id = _caseId;
    const [pageState, pageStateDispatch] = useReducer(CaseEditPageReducer, pageInitState);

    return (
        <div>
            <p>You clicked {pageState.info.id} times</p>
            <button onClick={() => pageStateDispatch({ type: 'increment' })}>Click me</button>
        </div>
    );
}

export default CaseEditPage;