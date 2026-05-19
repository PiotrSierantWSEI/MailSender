/* generated using openapi-typescript-codegen -- do not edit */
/* istanbul ignore file */
/* tslint:disable */
/* eslint-disable */
import type { IssueTokenRequest } from '../models/IssueTokenRequest';
import type { MailSendRequest } from '../models/MailSendRequest';
import type { RegisterClientAppRequest } from '../models/RegisterClientAppRequest';
import type { Void } from '../models/Void';
import type { CancelablePromise } from '../core/CancelablePromise';
import { OpenAPI } from '../core/OpenAPI';
import { request as __request } from '../core/request';
export class MailSenderService {
    /**
     * @param requestBody
     * @returns Void OK
     * @throws ApiError
     */
    public static issueToken(
        requestBody: IssueTokenRequest,
    ): CancelablePromise<Void> {
        return __request(OpenAPI, {
            method: 'POST',
            url: '/auth/token',
            body: requestBody,
            mediaType: 'application/json',
        });
    }
    /**
     * @param requestBody
     * @returns Void OK
     * @throws ApiError
     */
    public static registerClientApp(
        requestBody: RegisterClientAppRequest,
    ): CancelablePromise<Void> {
        return __request(OpenAPI, {
            method: 'POST',
            url: '/client-app/register',
            body: requestBody,
            mediaType: 'application/json',
        });
    }
    /**
     * @param requestBody
     * @returns Void OK
     * @throws ApiError
     */
    public static sendMessage(
        requestBody: MailSendRequest,
    ): CancelablePromise<Void> {
        return __request(OpenAPI, {
            method: 'POST',
            url: '/mail/send',
            body: requestBody,
            mediaType: 'application/json',
        });
    }
}
