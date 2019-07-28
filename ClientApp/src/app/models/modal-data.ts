export class ModalData {
  constructor(
    _header: string,
    _message: string,
    _buttonPositive: string,
    _buttonNegative: string = null
  ) {
    this.header = _header;
    this.message = _message;
    this.buttonPositive = _buttonPositive;
    if (_buttonNegative)
      this.buttonNegative = _buttonNegative;
  }

  public header: string;
  public message: string;
  public buttonPositive: string;
  public buttonNegative: string;

  public buttonPositiveHandler: Function;
  public buttonNegativeHandler: Function;
}
