// Disable/enable bodyscroll for overlay

var _bodyScrollDisabledCount = 0;

export function enableBodyScroll() {
    if (_bodyScrollDisabledCount > 0) {

        if (_bodyScrollDisabledCount === 1) {
            document.body.scroll = "yes";
            document.documentElement.style.overflow = 'auto';
            document.body.classList.remove("disabledBodyScroll");
            document.body.removeEventListener('touchmove', _disableIosBodyScroll);
        }

        _bodyScrollDisabledCount--;
    }
}

export function disableBodyScroll() {
    if (!_bodyScrollDisabledCount) {
        document.body.scroll = "no";
        document.documentElement.style.overflow = 'hidden';
        document.body.classList.add("disabledBodyScroll");
        document.body.addEventListener('touchmove', _disableIosBodyScroll, { passive: false, capture: false });
    }

    _bodyScrollDisabledCount++;
}

const _disableIosBodyScroll = (event) => {
    event.preventDefault();
};

// end