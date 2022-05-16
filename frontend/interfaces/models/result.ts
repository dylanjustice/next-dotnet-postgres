export interface Result<T> {
    errorCount: boolean;
    errors: Array<string>;
    nextLinkParams: any;
    resultObject: T;
}