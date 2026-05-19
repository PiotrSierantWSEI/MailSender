import { OpenAPI } from '../core/OpenAPI';
import { request as __request } from '../core/request';
export class MailSenderService {
    /**
     * @param requestBody
     * @returns Void OK
     * @throws ApiError
     */
    static issueToken(requestBody) {
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
    static registerClientApp(requestBody) {
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
    static sendMessage(requestBody) {
        return __request(OpenAPI, {
            method: 'POST',
            url: '/mail/send',
            body: requestBody,
            mediaType: 'application/json',
        });
    }
}
//# sourceMappingURL=MailSenderService.js.map