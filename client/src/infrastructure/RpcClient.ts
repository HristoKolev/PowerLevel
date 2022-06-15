// eslint-disable-next-line eslint-comments/disable-enable-pair
/* eslint-disable prettier/prettier */
import { RpcClientHelper } from '~infrastructure/RpcClientHelper';

export interface LoginRequest {
  emailAddress: string;
  password: string;
  rememberMe: boolean;
}

export interface ProfileInfoRequest {
  count: number;
}

export interface RegisterRequest {
  emailAddress: string;
  password: string;
}

export interface LoginResponse {
  csrfToken: string;
  emailAddress: string;
  userProfileID: number;
}

export interface PingResponse {
  message: string;
}

export interface ProfileInfoResponse {
  count: number;
}

export interface LoginError {
  userNotVerified?: boolean;
  errorMessages: string[];
  errorID?: string;
}

export interface DefaultApiError {
  errorMessages: string[];
  errorID?: string;
}

export type ApiResult<TPayload = null, TError extends DefaultApiError = DefaultApiError> =
  { isOk: true; payload: TPayload } | { isOk: false; error: TError };

export class RpcClient {
  private helper: RpcClientHelper;

  constructor(helper: RpcClientHelper) {
    this.helper = helper;
  }

  login(request: LoginRequest): Promise<ApiResult<LoginResponse, LoginError>> {
    return this.helper.send('LoginRequest', request);
  }

  ping(): Promise<ApiResult<PingResponse>> {
    return this.helper.send('PingRequest');
  }

  profileInfo(request: ProfileInfoRequest): Promise<ApiResult<ProfileInfoResponse>> {
    return this.helper.send('ProfileInfoRequest', request);
  }

  register(request: RegisterRequest): Promise<ApiResult> {
    return this.helper.send('RegisterRequest', request);
  }
}
