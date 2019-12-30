import pageInitState from '../initial_states/caseEditPageInitState';
import { useCaseEditPageReducer } from '../reducers/caseEditPageReducer';
import * as actionTypes from '../actions/actionTypes';
import React, { useEffect } from 'react';
import { useForm, Controller } from "react-hook-form";
import { ICaseEditPageState } from '../type_defs/ICaseEditPageState';
import Select from 'react-select';
import { IComment } from '../type_defs/IComment';
import { addComment } from '../server_mediators/comments';

// getting it from variable initialized from view bag
declare var _caseId: number;

function CaseEditPage() {
    const { register, handleSubmit, errors, setValue, reset, watch } = useForm();
    const onSubmit = data => console.log(data);
    const onCommentSubmit = async data => {
        console.log("New Comment inp data");
        console.log(data);
        pageStateDispatch({ type: actionTypes.addCommentAction, payload: data })
    };
    const handleAgenciesChange = val => setValue("concernedAgencies", val);
    const handleTagChange = val => setValue("commTag", val);

    console.log(errors);
    const agencySelectValue = watch("concernedAgencies");
    const tagSelectValue = watch("commTag");
    useEffect(() => {
        register({ name: "concernedAgencies" });
        register({ name: "commTag" });
    }, [register]);

    pageInitState.info.id = _caseId;
    let [pageState, pageStateDispatch] = useCaseEditPageReducer(pageInitState);

    return (
        <>
            {/*
            <div>
                <p>Id is {pageState.info.id}</p>
                <button onClick={() => pageStateDispatch({ type: actionTypes.incrementAction })}>+1</button>
                <button onClick={() => pageStateDispatch({ type: actionTypes.decrementAction })}>-1</button>
            </div>
            <div>
                <pre>{JSON.stringify(pageState, null, 4)}</pre>
            </div>
            */}
            <form onSubmit={handleSubmit(onSubmit)}>
                {pageState.info.caseItems.map((caseItem, caseItemInd) => {
                    const labelComp = (<>
                        <label key={`caseQues_${caseItemInd}`} className="question">{caseItem.question}</label>
                        <input ref={register} defaultValue={caseItem.id} name={`caseItemId[${caseItemInd}]`} key={`caseId_${caseItemInd}`} style={{ display: "none" }}></input>
                    </>);
                    let inpComp = <></>;
                    const formItemName = `caseItemResp[${caseItemInd}]`;
                    const defValue = caseItem.response;
                    if ([0, 3, 4].includes(caseItem.responseType)) { // ShortText, Choices, ChoicesWithText
                        inpComp = <input key={`caseResp_${caseItemInd}`} defaultValue={defValue} className="form-control" type="text" name={formItemName} ref={register({ maxLength: 250 })} />
                    } else if (caseItem.responseType == 1) { // LongText
                        inpComp = <textarea key={`caseResp_${caseItemInd}`} defaultValue={defValue} className="form-control" name={formItemName} ref={register({ maxLength: 700 })} />
                    } else if (caseItem.responseType == 2) { // DateTime
                        inpComp = <input key={`caseResp_${caseItemInd}`} defaultValue={defValue} className="form-control datetimepicker" name={formItemName} ref={register({ required: true })} />
                    }
                    return (
                        <>
                            {labelComp}
                            {inpComp}
                        </>
                    );
                })}
                <label className="question">Issue Time</label>
                <input className="form-control datetimepicker" defaultValue={pageState.info.downTime} name="issue_time" ref={register({ required: true })} />

                <label className="question">Concerned Agencies</label>
                {/*https://medium.com/@lahiru0561/react-select-with-custom-label-and-custom-search-122bfe06b6d7*/}

                {pageState.users.length > 0 &&
                    <Select value={agencySelectValue} onChange={handleAgenciesChange}
                        defaultValue={
                            pageState.users.filter(
                                us => { return pageState.info.concernedAgencies.some(ca => ca.userId == us.id) }
                            ).map(
                                usr => { return { label: usr.userName, value: usr.id } }
                            )
                        }
                        isMulti
                        options={pageState.users.map(us => { return { label: us.userName, value: us.id } })}
                        className="basic-multi-select"
                        classNamePrefix="select"
                        name="concernedAgencies"
                    />}

                <label className="question">Comments</label><br />
                {pageState.info.comments.length == 0 && <span>No comments recieved yet...</span>}
                {pageState.users.length > 0 &&
                    pageState.info.comments.map((comm, commInd) => {
                        return (<p>{`${comm.created} ${pageState.users.find(usr => (usr.id == comm.createdById)).userName} ${comm.tag.toString()} ${comm.comment}`}</p>)
                    }
                    )
                }
                <br />
                <button type="submit">Submit</button>
            </form>
            <form onSubmit={handleSubmit(onCommentSubmit)}>
                <span>Tag</span>
                {pageState.commentTagTypes.length > 0 &&
                    <Select value={tagSelectValue} onChange={handleTagChange}
                        className="basic-multi-select"
                        classNamePrefix="select"
                        options={pageState.commentTagTypes.map(tt => { return { label: tt, value: tt } })}
                        name="commTag"
                    />}

                <br />
                <span>Comment</span>
                <textarea name="comm" ref={register}></textarea>
                <button type="submit">Add Comment</button>
            </form>
        </>
    );
}

export default CaseEditPage;