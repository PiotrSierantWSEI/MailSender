import type { IssueTokenRequest } from '../models/IssueTokenRequest';
import type { MailSendRequest } from '../models/MailSendRequest';
import type { RegisterClientAppRequest } from '../models/RegisterClientAppRequest';
import type { Void } from '../models/Void';
import type { CancelablePromise } from '../core/CancelablePromise';
export declare class MailSenderService {
    /**
     * @param requestBody
     * @returns Void OK
     * @throws ApiError
     */
    static issueToken(requestBody: IssueTokenRequest): CancelablePromise<Void>;
    /**
     * @param requestBody
     * @returns Void OK
     * @throws ApiError
     */
    static registerClientApp(requestBody: RegisterClientAppRequest): CancelablePromise<Void>;
    /**
     * @param requestBody
     * @returns Void OK
     * @throws ApiError
     */
    static sendMessage(requestBody: MailSendRequest): CancelablePromise<Void>;
}
//# sourceMappingURL=MailSenderService.d.ts.map