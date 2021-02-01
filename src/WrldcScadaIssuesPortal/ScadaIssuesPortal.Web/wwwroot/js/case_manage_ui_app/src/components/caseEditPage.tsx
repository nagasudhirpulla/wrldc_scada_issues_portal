import pageInitState from '../initial_states/caseEditPageInitState';
import { useCaseEditPageReducer } from '../reducers/caseEditPageReducer';
import * as actionTypes from '../actions/actionTypes';
import React, { useEffect } from 'react';
import { useForm, Controller } from "react-hook-form";
import Select from 'react-select';
import { IComment } from '../type_defs/IComment';
import * as dateFormat from 'dateformat';

// getting it from variable initialized from view bag
declare var _caseId: number;
declare var $: any;

function CaseEditPage() {
    // setup reducer
    pageInitState.info.id = _caseId;
    let [pageState, pageStateDispatch] = useCaseEditPageReducer(pageInitState);

    //setup form
    const { register, handleSubmit, errors, setValue, reset, watch } = useForm();

    // setup concerned agencies multi select widget
    const handleAgenciesChange = val => setValue("concernedAgencies", val);
    const agencySelectValue = watch("concernedAgencies");

    useEffect(() => {
        register({ name: "concernedAgencies" });
    }, [register]);

    useEffect(() => {
        if (pageState.info.downTime) {
            $(".datetimepicker").flatpickr({
                enableTime: true,
                altInput: true,
                altFormat: "d-M-Y H:i",
            });
        }
    }, [pageState.info.downTime]);

    useEffect(() => {
        let defAgenciesOpts = pageState.users.filter(
            us => { return pageState.info.concernedAgencies.some(ca => ca.userId == us.id) }
        ).map(
            usr => { return { label: usr.userName, value: usr.id } }
        );
        console.log("defAgenciesOpts")
        console.log(defAgenciesOpts)
        handleAgenciesChange(defAgenciesOpts)
    }, [pageState.info.concernedAgencies, pageState.users]);

    // log errors
    console.log(errors);

    // on Issue edit form submit
    const onSubmit = data => {
        console.log(data)
        let caseObj = pageState.info;
        for (var caseItemIter = 0; caseItemIter < caseObj.caseItems.length; caseItemIter++) {
            // edit case item responses
            caseObj.caseItems[caseItemIter].response = data.caseItemResp[caseItemIter];
        }
        caseObj.downTime = data.issue_time;
        caseObj.concernedAgencies = data.concernedAgencies.map(ca => {
            return {
                reportingCaseId: pageState.info.id,
                userId: ca.value,
                id: -1
            }
        });
        console.log("caseObj=");
        console.log(caseObj);
        pageStateDispatch({ type: actionTypes.setCaseInfoAction, payload: caseObj })
    };

    // on New Comment Delete submit
    const onCommDel = (commId: number) => {
        return (() => {
            console.log(`deleleting comment id ${commId}`);
            pageStateDispatch({ type: actionTypes.delCommentAction, payload: commId })
        });
    };

    // on Attachment Delete submit
    const onAttachmentDel = () => {
        console.log(`deleleting attachment for reporting case id ${pageState.info.id}`);
        pageStateDispatch({ type: actionTypes.delAttachmentAction, payload: pageState.info.id })
    };

    // on New Comment form submit
    const onCommentSubmit = async data => {
        console.log("New Comment inp data");
        console.log(data);
        const newCommObj: IComment = {
            reportingCaseId: pageState.info.id,
            comment: data.comm,
            tag: pageState.commentTagTypes.findIndex(ct => ct == data.commTag),
            created: "",
            createdById: "",
            id: -1
        }
        pageStateDispatch({ type: actionTypes.addCommentAction, payload: newCommObj })
        setValue("comm", "");
    };

    // on New attachment form submit
    const onAddAttachmentSubmit = async data => {
        console.log("New attachment inp data");
        console.log(data);
        const formData = new FormData(document.getElementById("addAttachmentForm") as HTMLFormElement);
        pageStateDispatch({
            type: actionTypes.addAttachmentAction, payload: {
                id: formData.get("id"),
                caseAttachment: formData.get("caseAttachment")
            }
        });
        setValue("caseAttachment", "");
    };

    return (
        <>
            {/*<div><pre>{JSON.stringify(pageState, null, 4)}</pre></div>*/}
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
                {pageState.info.downTime && <input className="form-control datetimepicker" defaultValue={pageState.info.downTime} name="issue_time" ref={register({ required: true })} />}

                <label className="question">Concerned Agencies</label>
                {/*https://medium.com/@lahiru0561/react-select-with-custom-label-and-custom-search-122bfe06b6d7*/}

                {pageState.users.length > 0 &&
                    <Select value={agencySelectValue} onChange={handleAgenciesChange}
                        isMulti
                        options={pageState.users.map(us => { return { label: us.userName, value: us.id } })}
                        className="basic-multi-select"
                        classNamePrefix="select"
                        name="concernedAgencies"
                    />}

                {pageState.info.comments.length == 0 && <div><span>No comments recieved yet...</span><br /></div>}
                {pageState.info.comments.length > 0 && <label className="question">Comments</label>}
                {pageState.users.length > 0 &&
                    pageState.info.comments.sort((a, b) => (a.created > b.created) ? 1 : -1).map((comm, commInd) => {
                        return (
                            <div>
                                <span style={{ fontSize: "small", color: "cornflowerblue" }}>
                                    {(comm.tag == 1) && <span className="badge badge-danger"><i className="fas fa-ban"></i>{` ${pageState.commentTagTypes[comm.tag]}`}</span>}
                                    {(comm.tag == 2) && <span className="badge badge-success"><i className="fas fa-dot-circle"></i>{` ${pageState.commentTagTypes[comm.tag]}`}</span>}
                                    <b>{" "}{pageState.users.find(usr => (usr.id == comm.createdById)).userName}</b>
                                    {` on ${dateFormat(comm.created, 'mmm dd, yyyy HH:MM')}`}
                                </span>
                                <br />
                                <span>{`${comm.comment}`}</span>
                                <button className="btn btn-sm btn-link" style={{ color: "red" }} onClick={onCommDel(comm.id)} type="button">delete</button>
                                <br />
                            </div>
                        )
                    }
                    )
                }
                {pageState.info.attachmentName != null &&
                    <>
                        <span>Attachment - {pageState.info.attachmentName}</span>
                        <button className="btn btn-sm btn-link" onClick={onAttachmentDel} style={{ color: 'red' }} type="button">delete</button>
                    </>
                }
                <br />
                <button className="btn btn-sm btn-success" type="submit">Save Changes</button>
            </form>
            <br />
            {pageState.info.attachmentName == null &&
                <form encType="multipart/form-data" id="addAttachmentForm" onSubmit={handleSubmit(onAddAttachmentSubmit)}>
                    <input type="hidden" value={pageState.info.id} name="id" />
                    <span>Add Attachment -{" "}</span>
                    <input name="caseAttachment" type="file" />
                    <button className="btn btn-sm btn-link" style={{ color: 'green' }} type="submit">Submit</button>
                </form>
            }
            <br />
            <form onSubmit={handleSubmit(onCommentSubmit)}>
                <span className="h5" style={{ color: "#008b8b" }}>Add a Comment</span>
                <br />
                <span>Tag{" "}</span>
                <select className="select" name="commTag" ref={register({ required: true })}>
                    {pageState.commentTagTypes.map(tt => { return <option value={tt}>{tt}</option> })}
                </select>
                <br />
                <textarea name="comm" ref={register} style={{ minWidth: "60%", marginTop: "0.5em" }} placeholder="Enter a Comment..."></textarea>
                <br />
                <button className="btn btn-sm btn-info" type="submit">Add Comment</button>
            </form>
        </>
    );
}

export default CaseEditPage;