mergeInto(LibraryManager.library, {
    IsMobile: function() {
        return /iPhone|iPad|iPod|Android/i.test(navigator.userAgent);
    },

    IsSafari: function() {
        return /^((?!chrome|android).)*safari/i.test(navigator.userAgent);
    },

    // Keep existing functions, but modify InitializePointerLock:
    InitializePointerLock: function() {
        var lastX = 0;
        var lastY = 0;
        var accumulatedX = 0;
        var accumulatedY = 0;
        var isSafari = /^((?!chrome|android).)*safari/i.test(navigator.userAgent);
        var isMouseDown = false;

        // Create sensitivity calibration factors
        var baseFactor = 0.5;
        var safariFactorNormal = 0.25;
        var safariFactorDrag = 0.025;

        window.unityMouseFix = {
            getDeltaX: function() {
                var delta = accumulatedX;
                accumulatedX = 0;
                return delta;
            },
            getDeltaY: function() {
                var delta = accumulatedY;
                accumulatedY = 0;
                return delta;
            },
            isMouseButtonDown: function() {
                return isMouseDown;
            }
        };

        // Mouse event listeners remain the same...
        document.addEventListener('mousedown', function(e) {
            if (e.button === 0) { isMouseDown = true; }
        });

        document.addEventListener('mouseup', function(e) {
            if (e.button === 0) { isMouseDown = false; }
        });

        document.addEventListener('mouseleave', function() {
            isMouseDown = false;
        });

        document.addEventListener('pointermove', function(e) {
            if (document.pointerLockElement) {
                    var movementX = e.movementX;
                    var movementY = e.movementY;
            
                    // Only apply special sensitivity adjustments for Safari
                    if (isSafari) {
                        var factor = isMouseDown ? safariFactorDrag : safariFactorNormal;
                        movementX = movementX * factor;
                        movementY = movementY * factor;
                    } else {
                        // Default for all other browsers
                        movementX = movementX * baseFactor;
                        movementY = movementY * baseFactor;
                    }
            
                    // Rest of the function remains the same...
                    if (movementX !== undefined && movementY !== undefined) {
                        accumulatedX += movementX;
                        accumulatedY += movementY;
                    } else {
                    
                    var dx = e.clientX - lastX;
                    var dy = e.clientY - lastY;
        
                    // Same adjustment pattern for fallback method
                    if (isSafari) {
                        var factor = isMouseDown ? safariFactorDrag : safariFactorNormal;
                        dx *= factor;
                        dy *= factor;
                    } else {
                        dx *= baseFactor;
                        dy *= baseFactor;
                    }
        
                    accumulatedX += dx;
                    accumulatedY += dy;
                }
                
                lastX = e.clientX;
                lastY = e.clientY;
            }
        }, false);
    },

    GetMouseDeltaX: function() {
        return window.unityMouseFix ? window.unityMouseFix.getDeltaX() : 0;
    },

    GetMouseDeltaY: function() {
        return window.unityMouseFix ? -window.unityMouseFix.getDeltaY() : 0;
    }
});