// eslint-disable-next-line eslint-comments/disable-enable-pair
/* eslint-disable prettier/prettier */
import { BaseRpcClient, ApiResult } from './BaseRpcClient';

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

export class RpcClient {
  private baseClient: BaseRpcClient;

  constructor(baseClient: BaseRpcClient) {
    this.baseClient = baseClient;
  }

  login(request: LoginRequest): Promise<LoginResponse> {
    return this.baseClient.send('LoginRequest', request);
  }

  loginResult(request: LoginRequest): Promise<ApiResult<LoginResponse, LoginError>> {
    return this.baseClient.sendResult('LoginRequest', request);
  }

  ping(): Promise<PingResponse> {
    return this.baseClient.send('PingRequest');
  }

  pingResult(): Promise<ApiResult<PingResponse>> {
    return this.baseClient.sendResult('PingRequest');
  }

  profileInfo(request: ProfileInfoRequest): Promise<ProfileInfoResponse> {
    return this.baseClient.send('ProfileInfoRequest', request);
  }

  profileInfoResult(request: ProfileInfoRequest): Promise<ApiResult<ProfileInfoResponse>> {
    return this.baseClient.sendResult('ProfileInfoRequest', request);
  }

  register(request: RegisterRequest): Promise<void> {
    return this.baseClient.send('RegisterRequest', request);
  }

  registerResult(request: RegisterRequest): Promise<ApiResult> {
    return this.baseClient.sendResult('RegisterRequest', request);
  }
}
